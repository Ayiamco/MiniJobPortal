using inSpark.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inSpark.Infrastructure.Interfaces
{
    public interface IDbService<T1, T2> 
    //T1 is the DataEntity type, T2 is the data type of the Entity's Id
    {
        DbContext dbContext { get; set; }
        void CreateItem(T1 obj);

        T1 ReadItem(T2 id);

        void UpdateItem(T1 obj);
        void DeleteItem(T2 id);

    }
}
