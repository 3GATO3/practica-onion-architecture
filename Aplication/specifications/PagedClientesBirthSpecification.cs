using Ardalis.Specification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.specifications
{
    public class PagedClientesBirthSpecification : Specification<Cliente>
    {
        public PagedClientesBirthSpecification(int pageSize, int pageNumber, string birth)
        {
            Query.Skip((pageNumber-1)*pageSize)
                .Take(pageSize);

            if (!string.IsNullOrEmpty(birth))
            {
                Query.Where(x => x.FechaNacimiento.Month.ToString() == birth);
            }

          


        }
    }
}
