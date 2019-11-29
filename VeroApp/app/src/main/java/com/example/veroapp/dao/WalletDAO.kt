package com.example.veroapp.dao

import android.arch.persistence.room.Dao
import android.arch.persistence.room.Insert
import android.arch.persistence.room.Query
import com.example.veroapp.models.WalletModel

@Dao
interface WalletDAO {
    @Query("SELECT * FROM walletmodel LIMIT 1")
    fun get(): WalletModel
    @Insert
    fun add(vararg wallet: WalletModel)
}