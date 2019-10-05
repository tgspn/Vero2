package com.example.veroapp.Database

import android.arch.persistence.room.Database
import android.arch.persistence.room.RoomDatabase
import com.example.veroapp.dao.KeyDAO
import com.example.veroapp.dao.PessoaDAO
import com.example.veroapp.models.KeyModel
import com.example.veroapp.models.PessoaModel

@Database(entities=[KeyModel::class,PessoaModel::class],version = 2,exportSchema = false )
 abstract class AppDatabase() : RoomDatabase() {

    abstract fun keyDAO(): KeyDAO
    abstract fun pessoaDAO(): PessoaDAO
}