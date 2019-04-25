using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using XTC.Types;
using XTC.Logger;

public class SQLiteUtility
{
    public delegate void ExecuteHandle(SqliteDataReader _reader);

    private static Dictionary<string, SqliteConnection> connections = new Dictionary<string, SqliteConnection>();


    public static SqliteConnection Open(string _databaseFile, out Error _err)
    {
        _err = Error.OK;

        if (connections.ContainsKey(_databaseFile))
            return connections[_databaseFile];

        try
        {
            SqliteConnection connection = new SqliteConnection("data source=" + _databaseFile);
            connection.Open();
            connections[_databaseFile] = connection;
            return connection;
        }
        catch (Exception e)
        {
            Log.Exception("SQLiteUtility.Open", e);
            _err = new Error(-1, e.Message);
            return null;
        }
    }

    public static void Close(string _databaseFile, out Error _err)
    {
        _err = Error.OK;
        if (!connections.ContainsKey(_databaseFile))
        {
            _err = new Error(1, "already closed");
            return;
        }
        SqliteConnection connection = connections[_databaseFile];
        connection.Close();
        connections.Remove(_databaseFile);
    }

    public static Error Execute(SqliteConnection _connection, string _query)
    {
        return Execute(_connection, _query, (_reader)=>{});
    }

    public static Error Execute(SqliteConnection _connection, string _query, ExecuteHandle _handle)
    {
        if(null == _connection)
            return new Error(1, "connection is null");

        try
        {
			//one execute will create new command and reader.
			//close them after finish.
            SqliteCommand command = _connection.CreateCommand();
            command.CommandText = _query;
            SqliteDataReader reader = command.ExecuteReader();
			_handle(reader);
            reader.Close();
            command.Cancel();
            return Error.OK;
        }
        catch (System.Exception e)
        {
            Log.Exception("SQLiteUtility.Execute", e);
            return new Error(2, e.Message);
        }
    }
}