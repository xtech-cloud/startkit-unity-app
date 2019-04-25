using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using XTC.MVCS;
using XTC.Types;
using XTC.Logger;

public class AccountDAO
{
    private SqliteConnection connection
    {
        get
        {
            Error error;
            SqliteConnection conn = SQLiteUtility.Open(Constant.DataBasePath, out error);
            if (error != Error.OK)
                this.LogError(error.message);
            return conn;
        }
    }


    public void AutoMigrate()
    {
        string query = @"CREATE TABLE IF NOT EXISTS Account(
                            ID INTEGER PRIMARY KEY NOT NULL,
                            UUID CHAR(32) UNIQUE NOT NULL,
                            Profile TEXT NOT NULL,
                            CreatedAt CHAR(19) NOT NULL,
                            UpdatedAt  CHAR(19) NOT NULL
                        )";
        Error error = SQLiteUtility.Execute(connection, query);
        if (error != Error.OK)
        {
            this.LogError(error.message);
        }
    }

    public AccountModel.Account QueryProfile(string _accountID, out Error _err)
    {
        _err = Error.OK;
        
        AccountModel.Account account = new AccountModel.Account();
        string query = string.Format("SELECT * FROM Account WHERE UUID='{0}'", _accountID);
        _err = SQLiteUtility.Execute(connection, query, (_reader) =>
        {
            while(_reader.Read())
            {
                account.profile = _reader.GetString(_reader.GetOrdinal("Profile"));
            }
        });

        return account;
    }
    
    public Error Insert(AccountModel.Account _account)
    {
        string query = string.Format("INSERT INTO Account (UUID, Profile, CreatedAt, UpdatedAt) VALUES ('{0}', '{1}', '{2}', '{3}')",
         _account.accountID, _account.profile, ModelUtility.NewUtcNow(), ModelUtility.NewUtcNow());
        return SQLiteUtility.Execute(connection, query);
    }

    public Error Delete(AccountModel.Account _account)
    {
        string query = string.Format("DELETE FROM Account WHERE UUID='{0}'", _account.accountID);
        return SQLiteUtility.Execute(connection, query);
    }
}