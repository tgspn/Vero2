package com.example.veroapp.models

import android.arch.persistence.room.Entity
import android.arch.persistence.room.PrimaryKey
import java.util.*

@Entity
class KeyModel(

    val computerName: String,
    val date: String,
    val ip: String,
    @PrimaryKey(autoGenerate = false)
    val id: String= UUID.randomUUID().toString(),
    val pc_id: String

) {

}