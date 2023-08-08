using Booktopia.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Booktopia.Repository.Interface
{
    public interface IBookkRepository
    {
        Task<List<Book>> GetAll();
        Task<List<Book>> SearchByGenres(string Genre);
        Task<List<string>> GetAllGenres();
    }
}
