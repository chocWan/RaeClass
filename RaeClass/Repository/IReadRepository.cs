﻿using RaeClass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaeClass.Repository
{
    public interface IReadRepository
    {
        Task<int> AddAsync(string level, string name, string cncontent, string encontent, string recordFileId1, string recordFileId2);
        Task<int> UpdateAsync(string readNumber, string level, string name, string cncontent, string encontent, string recordFileId1, string recordFileId2);
        Tuple<List<Read>, int> GetPageListAsync(int pageindex, int pagesize, string level, string titleOrContent);
        Read GetRead(string readNumber);
        Read GetNewRead(string level, string name, string cncontent, string encontent, string recordFileId1, string recordFileId2);
    }
}
