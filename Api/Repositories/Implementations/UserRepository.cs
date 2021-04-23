using System.Threading.Tasks;
using Api.Models;
using Api.Repositories.Interfaces;
using Dapper;
using Npgsql;

namespace Api.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        NpgsqlConnection _connection;
        public UserRepository(NpgsqlConnection connection)
        {
            this._connection = connection;
        }

        public void Create(User user)
        {
            string sql = @"insert into users 
            (id, name, email, password, default_delivery_adress, creation_date, last_update_date)
            values
            (@id, @name, @email, @password, @default_delivery_adress, @creation_date, @last_update_date)";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, this._connection))
            {
                command.Parameters.Add("id", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.Id;
                command.Parameters.Add("name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.Name;
                command.Parameters.Add("email", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.Email;
                command.Parameters.Add("password", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.Password;
                command.Parameters.Add("default_delivery_adress", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.DefaultDeliveryAdress;
                command.Parameters.Add("creation_date", NpgsqlTypes.NpgsqlDbType.Date).Value = user.CreationDate;
                command.Parameters.Add("last_update_date", NpgsqlTypes.NpgsqlDbType.Date).Value = user.LastUpdateDate;
                this._connection.Open();
                command.ExecuteNonQuery();
                this._connection.Close();
            }
        }

        public void Deactivate(string id)
        {
            throw new System.NotImplementedException();
        }

        public User FindByEmail(string email)
        {
            string sql = @"select 
            id, 
            name, 
            email, 
            password, 
            default_delivery_adress as defaultdeliveryadress, 
            creation_date as creationdate,
            last_update_date as lastupdatedate 
            from users 
            where email = @email";
            this._connection.Open();
            var user = this._connection.QueryFirstOrDefault<User>(sql, new { email = email });
            this._connection.Close();
            return user;
        }

        public User FindById(string id)
        {
            string sql = @"select 
            id, 
            name, 
            email, 
            password, 
            default_delivery_adress as defaultdeliveryadress, 
            creation_date as creationdate,
            last_update_date as lastupdatedate 
            from users 
            where id = @id";
            this._connection.Open();
            var user = this._connection.QueryFirstOrDefault<User>(sql, new { id = id });
            this._connection.Close();
            return user;
        }

        public void Update(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}