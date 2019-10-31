package com.example.veroapp.models

import android.arch.persistence.room.Entity
import android.arch.persistence.room.PrimaryKey
@Entity
class WalletModel (var wallet:String,
                   @PrimaryKey(autoGenerate = true)
                   val id:Int=0) {
}