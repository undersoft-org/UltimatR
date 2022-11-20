//-----------------------------------------------------------------------
// <copyright file="Entity.cs" company="Undersoft">
//     Author: Dariusz Hanc
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Instant;
using System.Runtime.InteropServices;
using System.Uniques;

namespace UltimatR
{
    [StructLayout(LayoutKind.Sequential)]
    public class Entity : Identifiable, IEntity, INotifyPropertyChanged
    {

        public Entity()
        {
            CompileValuator();
        }

        public event PropertyChangedEventHandler PropertyChanged;               
    }
}
