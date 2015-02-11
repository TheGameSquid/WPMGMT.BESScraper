using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PetaPoco;

namespace WPMGMT.BESScraper
{
    class BesDb
    {
        private string connectionStringName;

        public string ConnectionStringName { get; set; }

        public T InsertOrUpdate<T>(T objPOCO) where T : new()
        {
            Database db = new PetaPoco.Database(this.ConnectionStringName);


            var response = client.Execute<T>(request);

            try
            {
                if (response.ErrorException != null)
                {
                    throw new Exception(response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error encountered: {0}", ex.Message);
            }

            return response.Data;
        }

        private bool Exists<T>(T objPOCO) where T : new()
        {
            Database db = new PetaPoco.Database(this.ConnectionStringName);
            return db.Exists<T>(String.Format("ID = '1855'"), objPOCO.ID);
        }
    }
}
