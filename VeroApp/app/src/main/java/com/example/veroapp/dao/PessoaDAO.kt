package com.example.veroapp.dao

import android.arch.persistence.room.Dao
import android.arch.persistence.room.Insert
import android.arch.persistence.room.Query
import android.arch.persistence.room.Update
import com.example.veroapp.models.KeyModel
import com.example.veroapp.models.PessoaModel

@Dao
interface PessoaDAO {
    @Query("SELECT * FROM pessoamodel")
    fun all(): List<PessoaModel>
    @Insert
    fun add(vararg keymodel: PessoaModel)
    @Query("SELECT * FROM pessoamodel LIMIT 1")
    fun get():PessoaModel
    @Update
    fun update(vararg keymodel: PessoaModel)
    @Query("SELECT :field FROM pessoamodel LIMIT 1")
    fun getFieldValue(field:String):String
}
