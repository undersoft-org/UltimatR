using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Instant;
using System.Linq;
using System.Linq.Expressions;

namespace UltimatR
{
    public class EntityValidation<TEntity> : AbstractValidator<EntityCommand<TEntity>> where TEntity : Entity
    {
        private static readonly string[] SupportedLanguages;

        private readonly IUltimateService _ultimate;

        static EntityValidation()
        {
            SupportedLanguages = CultureInfo.GetCultures(CultureTypes.NeutralCultures)
                .Select(c => c.TwoLetterISOLanguageName).Distinct().ToArray();
        }

        protected EntityValidation(IUltimateService ultimate)
        {
            _ultimate = ultimate;
        }

        protected void ValidateNotEmpty(Expression<Func<EntityCommand<TEntity>, object>> member)
        {
            RuleFor(member)
                .NotEmpty().WithMessage(
                    $"{member.Parameters.FirstOrDefault().GetType().GetGenericArguments().FirstOrDefault().Name} is required!");
        }

        protected void ValidateNotEmpty(params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                RuleFor(a => a.Data.ValueOf(propertyName))
                    .NotEmpty().WithMessage(a => 
                        $"{propertyName} is required!");
            }
        }

        protected void ValidateEmail(Expression<Func<EntityCommand<TEntity>, string>> member)
        {
            RuleFor(member)
                    .EmailAddress()
                    .When(a => !string.IsNullOrEmpty(a.Data.ValueOf(member.Parameters.FirstOrDefault().GetType().GetGenericArguments().FirstOrDefault().Name).ToString()))
                    .WithMessage($"Invalid email address.");
        }

        protected void ValidateEmail(params string[] emailPropertyNames)
        {
            foreach (string emailPropertyName in emailPropertyNames)
            {
                RuleFor(a => a.Data.ValueOf(emailPropertyName).ToString())
                    .EmailAddress()
                    .When(a => !string.IsNullOrEmpty(a.Data.ValueOf(emailPropertyName).ToString()))
                    .WithMessage($"Invalid {emailPropertyName} address.");
            }
        }

        protected void ValidateLength(int min, int max, Expression<Func<EntityCommand<TEntity>, string>> member)
        {
            RuleFor(member)
                .MinimumLength(min)
                .WithMessage($"minimum text length: {max} characters")
                .MaximumLength(max)
                .WithMessage($"maximum text length: {max} characters");
        }

        protected void ValidateLength(int min, int max, params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                RuleFor(a => a.Data.ValueOf(propertyName).ToString())
                    .MinimumLength(min)
                    .WithMessage($"{propertyName} minimum text length: {max} characters")
                    .MaximumLength(max)
                    .WithMessage($"{propertyName} maximum text length: {max} characters")
                    .When(a => !string.IsNullOrEmpty(a.Data.ValueOf(propertyName).ToString()));
            }
        }

        protected void ValidateEnum(params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                RuleFor(e => e.Data.ValueOf(propertyName))
                    .IsInEnum().WithMessage($"Incorrect {propertyName} number");
            }
        }

        protected void ValidateEnum(Expression<Func<EntityCommand<TEntity>, string>> member)
        {
            RuleFor(member)
                .IsInEnum().WithMessage($"Incorrect enum value");
        }

        protected void ValidateNotEqual(object item, params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                RuleFor(e => e.Data.ValueOf(propertyName))
                    .NotEqual(item).WithMessage($"{propertyName} is not equal: {item}");
            }
        }

        protected void ValidateNotEqual(object item, Expression<Func<EntityCommand<TEntity>, object>> member)
        {
            RuleFor(member)
                .NotEqual(item).WithMessage($"value not equal: {item}");
        }

        protected void ValidateLanguage(Expression<Func<EntityCommand<TEntity>, object>> member)
        {
            RuleFor(member)
                .Must(SupportedLanguages.Contains).WithMessage("Agreement language must conform to ISO 639-1.");
        }

        protected void ValidateLanguage(params string[] propertyNames)
        {
            foreach (string propertyName in propertyNames)
            {
                RuleFor(e => e.Data.ValueOf(propertyName))
                    .Must(SupportedLanguages.Contains).WithMessage("Agreement language must conform to ISO 639-1.");
            }
        }

        protected void ValidateExist<TStore>(Func<EntityCommand<TEntity>, Expression<Func<TEntity, bool>>> command) where TStore : IDataStore            
        {
            RuleFor(e => e)
                .MustAsync(async (cmd, cancel) =>
                {
                    Expression<Func<TEntity, bool>> predicate = command(cmd);
                    return await _ultimate.use<TStore, TEntity>().Exist(predicate);

                }).WithMessage($"{typeof(TEntity).Name} does not exists");            
        }

        protected void ValidateNotExist<TStore>(Func<EntityCommand<TEntity>, Expression<Func<TEntity, bool>>> command) where TStore : IDataStore          
        {
            RuleFor(e => e)
                .MustAsync(async (cmd, cancel) =>
                {
                    Expression<Func<TEntity, bool>> predicate = command(cmd);
                    return await _ultimate.use<TStore, TEntity>().NotExist(predicate);

                }).WithMessage($"{typeof(TEntity).Name} already exists");
        }     
    }
}