/*************************************************
   Copyright (c) 2021 Undersoft

   System.Instant.Tests.InstantFigureTest.cs.Tests
   
   @project: Vegas.Sdk
   @stage: Development
   @author: Dariusz Hanc
   @date: (05.06.2021) 
   @licence MIT
 *************************************************/

using System;
using System.Collections.Generic;
using System.Instant;
using System.Uniques;
using Xunit;

namespace System.Instant.Tests
{
    public class SleeveTest
    {
        #region Methods

        [Fact]
        public void Sleeve_Test_For_Compilated_Type_With_GetByName()
        {
            var profile = new Agreement()
            {
                Comments = "fdfdsgg", CreationTime = DateTime.Now, Kind = AgreementKind.Agree, Creator = "sfssd",
                VersionId = 222
            };

            var sleeve = new Sleeve<Agreement>();

            profile.AutoId();

            var _sleeve0 = profile.ToSleeve();
            var _sleeve1 = sleeve.Combine(_sleeve0);

            
            var _figures = new Figures(_sleeve1);

            var mock = new Agreement()
            {
                Comments = "fdsfsf", CreationTime = DateTime.Now, Kind = AgreementKind.Cancellation, Creator = "fsds",
                VersionId = 992
            };
            
            var prop = sleeve.Rubrics;
            List<ISleeve> list = new();
            for (int i = 0; i < 300000; i++)
            {
                var _sleeve2 = sleeve.Combine();
                foreach (var rubric in prop)
                {
                    _sleeve2[rubric.RubricName] = _sleeve0[rubric.RubricName];
                }
                list.Add(_sleeve2);
            }

            _sleeve1["TypeId"] = 1005UL;
            object o = _sleeve1[5];
           
        }

        [Fact]
        private void Sleeve_Test_For_Compilated_Type_With_GetById()
        {
            var profile = new Agreement()
            {
                Comments = "fdfdsgg", CreationTime = DateTime.Now, Kind = AgreementKind.Agree, Creator = "sfssd",
                VersionId = 222
            };

            var sleeve = new Sleeve<Agreement>();

            profile.AutoId();

            var _sleeve0 = sleeve.Combine(profile);

            var _sleeve1 = sleeve.Combine(_sleeve0);

            var mock = new Agreement()
            {
                Comments = "fdsfsf", CreationTime = DateTime.Now, Kind = AgreementKind.Cancellation, Creator = "fsds",
                VersionId = 992
            };
            
            var prop = sleeve.Rubrics;
            List<ISleeve> list = new();
            for (int i = 0; i < 300000; i++)
            {
                var _sleeve2 = sleeve.Combine();
                for (int j = 0; j < prop.Count; j++)
                {
                    _sleeve2[j] = _sleeve0[j];
                }
                list.Add(_sleeve2);
            }

            _sleeve1["TypeId"] = 1005UL;
            object o = _sleeve1[5];
           
        }     

        #endregion
    }
}
