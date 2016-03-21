using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace TVItems.DAta
{
    public class TVItemsRepo
    {
        private readonly string _connectionString;

        public TVItemsRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<TVItem> GetAll()
        {
            using (var dataContext = new TVItemsDataContext(_connectionString))
            {
                return dataContext.TVItems.ToList();
            }
        }

        public void AddItem(TVItem item)
        {
            using (var dataContext = new TVItemsDataContext(_connectionString))
            {
                dataContext.TVItems.InsertOnSubmit(item);
                dataContext.SubmitChanges();
            }
        }

        public void Update(TVItem item)
        {
            using (var dataContext = new TVItemsDataContext(_connectionString))
            {
                dataContext.TVItems.Attach(item);
                dataContext.Refresh(RefreshMode.KeepCurrentValues, item);
                dataContext.SubmitChanges();
            }
        }

        public void Delete(int itemNumber)
        {
            using (var dataContext = new TVItemsDataContext(_connectionString))
            {
                dataContext.ExecuteCommand("DELETE FROM TVItems WHERE ItemNumber = {0}", itemNumber);
            }
        }

        public void AddChangeLog(ChangeLog changeLog)
        {
            using (var dataContext = new TVItemsDataContext(_connectionString))
            {
                dataContext.ChangeLogs.InsertOnSubmit(changeLog);
                dataContext.SubmitChanges();
            }
        }

        public void ClearChangeLogs()
        {
            using (var dataContext = new TVItemsDataContext(_connectionString))
            {
                dataContext.ExecuteCommand("TRUNCATE TABLE ChangeLogs");
            }
        }
    }
}
