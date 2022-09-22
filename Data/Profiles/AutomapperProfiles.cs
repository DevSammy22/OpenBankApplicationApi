using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Data.Dtos;
using Domain.Models;

namespace Data.Profiles
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<RegisterNewAccountModel, Account>();
            CreateMap<UpdateAccountModel, Account>();
            CreateMap<Account, GetAccountModel>();
            CreateMap<TransactionRequestDto, Transaction>();
        }
    }
}
