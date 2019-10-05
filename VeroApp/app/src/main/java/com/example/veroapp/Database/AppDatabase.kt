package com.example.veroapp.Database

import android.arch.persistence.room.Database
import android.arch.persistence.room.Room
import android.arch.persistence.room.RoomDatabase
import android.content.Context
import com.example.veroapp.dao.KeyDAO
import com.example.veroapp.dao.PessoaDAO
import com.example.veroapp.models.KeyModel
import com.example.veroapp.models.PessoaModel

@Database(entities=[KeyModel::class,PessoaModel::class],version = 2,exportSchema = false )
 abstract class AppDatabase() : RoomDatabase() {

    abstract fun keyDAO(): KeyDAO
    abstract fun pessoaDAO(): PessoaDAO
    companion object : SingletonHolder<AppDatabase, Context>({
        Room.databaseBuilder(it.applicationContext,
            AppDatabase::class.java, "vero-database")
            .build()
    })
}