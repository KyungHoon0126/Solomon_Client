﻿using MySql.Data.MySqlClient;
using Solomon_Client.DataBase;
using System.Data;

namespace Solomon_Client.Database
{
    // TODO : Repository Public 시, DB 정보 제외.
    public class MySqlDBConnectionManager : DBConnectionManager
    {
        private readonly string DATA_BASE_URL = $"SERVER=localhost;DATABASE=Bulletin;UID=root;PASSWORD=#kkh03kkh#;allow user variables=true";

        public override IDbConnection GetConnection()
        {
            IDbConnection db = new MySqlConnection(DATA_BASE_URL);
            return db;
        }
    }
}
