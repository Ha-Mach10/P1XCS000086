﻿using System;
using System.Collections.Generic;
using System.Text;

namespace P1XCS000086.Services.Interfaces.Sql
{
    public interface ISqlInsert
    {
        // Properties
        public string ResultMessage { get; }
        public string ExceptionMessage { get; }


        // Methods
        public bool Insert(string connStr, string command, List<string> columns, List<string> values);

		public bool Insert(string command, List<string> columns, List<string> values);
    }
}
