
using FluentValidation;
using MediatR;
using System;
using System.Instant;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using IdentityModel;

/// <summary>
/// The UltimatR namespace.
/// </summary>
namespace UltimatR
{
    /// <summary>
    /// Class CommandSetValidator.
    /// Implements the <see cref="FluentValidation.AbstractValidator{TCommand}" />
    /// </summary>
    /// <typeparam name="TCommand">The type of the t command.</typeparam>
    /// <seealso cref="FluentValidation.AbstractValidator{TCommand}" />
    public abstract class CommandSetValidator<TCommand> : AbstractValidator<TCommand> where TCommand : ICommandSet
    {
        /// <summary>
        /// The supported languages
        /// </summary>
        protected static readonly string[] SupportedLanguages;

        /// <summary>
        /// The uservice
        /// </summary>
        protected readonly IUltimateService uservice;

        /// <summary>
        /// Initializes static members of the <see cref="CommandSetValidator{TCommand}"/> class.
        /// </summary>
        static CommandSetValidator()
        {
            SupportedLanguages = CultureInfo.GetCultures(CultureTypes.NeutralCultures)
                         .Select(c => c.TwoLetterISOLanguageName).Distinct().ToArray();           
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandSetValidator{TCommand}"/> class.
        /// </summary>
        /// <param name="ultimateService">The ultimate service.</param>
        public CommandSetValidator(IUltimateService ultimateService)
        {
            uservice = ultimateService;
        }

        /// <summary>
        /// Validates the limit.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        protected virtual void ValidateLimit(int min, int max)
        {
            RuleFor(a => a)
                 .Must(a => a.Commands.Count() >= min)
                 .WithMessage($"Items count below minimum quantity")
                 .Must(a => a.Commands.Count() <= max)
                 .WithMessage($"Items count above maximum quantity");
        }

        /// <summary>
        /// Validates the required.
        /// </summary>
        /// <param name="propertyNames">The property names.</param>
        protected void ValidateRequired(params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {                
                RuleForEach(a => a.Commands)
                    .ChildRules(c => c
                    .RuleFor(r => r.Data
                    .ValueOf(propertyName))
                    .NotEmpty()
                    .WithMessage(a =>
                        $"{propertyName} is required!"));
            }
        }

        /// <summary>
        /// Validates the language.
        /// </summary>
        /// <param name="propertyNames">The property names.</param>
        protected void ValidateLanguage(params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                RuleForEach(a => a.Commands)
                  .ChildRules(c => c
                  .RuleFor(r => r.Data
                  .ValueOf(propertyName))
                .Must(SupportedLanguages.Contains).WithMessage("Agreement language must conform to ISO 639-1."));
            }
        }

        /// <summary>
        /// Validates the equal.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="propertyNames">The property names.</param>
        protected void ValidateEqual(object item, params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                RuleForEach(a => a.Commands)
                 .ChildRules(c => c
                 .RuleFor(r => r.Data
                 .ValueOf(propertyName))
                 .Equal(item).WithMessage($"{propertyName} is equal: {item}"));
            }
        }

        /// <summary>
        /// Validates the not equal.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="propertyNames">The property names.</param>
        protected void ValidateNotEqual(object item, params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                RuleForEach(a => a.Commands)
               .ChildRules(c => c
               .RuleFor(r => r.Data
               .ValueOf(propertyName))
               .NotEqual(item).WithMessage($"{propertyName} is not equal: {item}"));
            }
        }

        /// <summary>
        /// Validates the length.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="propertyNames">The property names.</param>
        protected void ValidateLength(int min, int max, params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                RuleForEach(a => a.Commands)
                .ChildRules(c => c
                .RuleFor(r => r.Data
                .ValueOf(propertyName).ToString())
                .MinimumLength(min)
                    .WithMessage($"{propertyName} minimum text length: {max} characters")
                    .MaximumLength(max)
                    .WithMessage($"{propertyName} maximum text length: {max} characters"));
            }
        }

        /// <summary>
        /// Validates the enum.
        /// </summary>
        /// <param name="propertyNames">The property names.</param>
        protected void ValidateEnum(params string[] propertyNames)
        {           
            foreach (string propertyName in propertyNames)
            {
                RuleForEach(a => a.Commands)
                .ChildRules(c => c
                .RuleFor(r => r.Data.ValueOf(propertyName))
                .IsInEnum().WithMessage($"Incorrect {propertyName} number"));
            }
        }

        /// <summary>
        /// Validates the email.
        /// </summary>
        /// <param name="emailPropertyNames">The email property names.</param>
        protected void ValidateEmail(params string[] emailPropertyNames)
        {
            foreach (string emailPropertyName in emailPropertyNames)
            {
                RuleForEach(a => a.Commands)
                .ChildRules(c => c
                .RuleFor(r => r.Data
                .ValueOf(emailPropertyName).ToString())
                .EmailAddress()            
                    .WithMessage($"Invalid {emailPropertyName} address."));
            }
        }

        /// <summary>
        /// Validates the exist.
        /// </summary>
        /// <typeparam name="TStore">The type of the t store.</typeparam>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="operand">The operand.</param>
        /// <param name="propertyNames">The property names.</param>
        protected void ValidateExist<TStore, TEntity>(LogicOperand operand, params string[] propertyNames)
            where TEntity : Entity where TStore : IDataStore
        {
            var _repository = uservice.Use<TStore, TEntity>();

            RuleForEach(a => a.Commands)
              .ChildRules(c => c
              .RuleFor(r => r.Data) // 
              .MustAsync(async (cmd, cancel) =>
                {
                    return await _repository.Exist(buildPredicate<TEntity>
                                                  ((IValueProxy)cmd, operand, propertyNames));

                }).WithMessage($"{typeof(TEntity).Name} already exists"));
        }

        /// <summary>
        /// Validates the not exist.
        /// </summary>
        /// <typeparam name="TStore">The type of the t store.</typeparam>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="operand">The operand.</param>
        /// <param name="propertyNames">The property names.</param>
        protected void ValidateNotExist<TStore, TEntity>(LogicOperand operand, params string[] propertyNames)
            where TEntity : Entity where TStore : IDataStore
        {
            var _repository = uservice.Use<TStore, TEntity>();

            RuleForEach(a => a.Commands)
               .ChildRules(c => c
               .RuleFor(r => r.Data)
                .MustAsync(async (cmd, cancel) =>
                {
                    return await _repository.NotExist(buildPredicate<TEntity>
                                                     ((IValueProxy)cmd, operand, propertyNames));

                }).WithMessage($"{typeof(TEntity).Name} does not exists"));
        }

        /// <summary>
        /// Builds the predicate.
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <param name="dataInput">The data input.</param>
        /// <param name="operand">The operand.</param>
        /// <param name="propertyNames">The property names.</param>
        /// <returns>Expression&lt;Func&lt;TEntity, System.Boolean&gt;&gt;.</returns>
        private Expression<Func<TEntity, bool>> buildPredicate<TEntity>(IValueProxy dataInput, LogicOperand operand, params string[] propertyNames)
              where TEntity : IValueProxy
        {
            Expression<Func<TEntity, bool>> predicate = (operand == LogicOperand.And)
                                                       ? predicate = e => true
                                                       : predicate = e => false;
            foreach (var item in propertyNames)
            {
                predicate = (operand == LogicOperand.And)
                                  ? predicate.And(e => e[item] == dataInput[item])
                                  : predicate.Or(e => e[item] == dataInput[item]);
            }
            return predicate;
        }
    }
}