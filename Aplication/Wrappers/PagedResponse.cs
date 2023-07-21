using Aplication.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Wrappers
{
    public class PagedResponse<T>: Response<T>
    {
        private ClienteDto clientesDto;

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;   
            this.Data = data;
            this.Message = null;
            this.Succeded = true;
            this.Errors = null;
        }

        //public PagedResponse(ClienteDto clientesDto, int pageNumber, int pageSize)
        //{
        //    this.clientesDto = clientesDto;
        //    PageNumber = pageNumber;
        //    PageSize = pageSize;
        //}
    }
}
