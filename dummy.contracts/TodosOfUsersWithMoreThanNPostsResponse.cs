using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dummy.contracts
{
    public class TodosOfUsersWithMoreThanNPostsResponse
    {
        public List<TodoModel> Todos { get; set; }
    }
}
