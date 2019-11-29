package com.example.veroapp.dao

import android.arch.persistence.room.Dao
import android.arch.persistence.room.Insert
import android.arch.persistence.room.Query
import com.example.veroapp.models.KeyModel
@Dao
interface  KeyDAO {
    @Query("SELECT * FROM keymodel")
    fun all(): MutableList<KeyModel>
    @Insert
    fun add(vararg keymodel: KeyModel)
    @Query("SELECT COUNT(*) FROM keymodel")
    fun count():Int
}