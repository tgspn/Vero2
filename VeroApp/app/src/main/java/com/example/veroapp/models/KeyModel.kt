package com.example.veroapp.models

import android.arch.persistence.room.Entity
import android.arch.persistence.room.PrimaryKey

@Entity
class KeyModel(

    val computerName: String,
    val date: String,
    val ip: String,
    @PrimaryKey(autoGenerate = true)
    val id: Long = 0,
    val pc_id: String

) {

}