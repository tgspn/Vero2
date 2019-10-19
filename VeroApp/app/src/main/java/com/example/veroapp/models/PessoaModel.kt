package com.example.veroapp.models

import android.arch.persistence.room.Entity
import android.arch.persistence.room.PrimaryKey

@Entity
class PessoaModel(
    @PrimaryKey(autoGenerate = true)
    val id: Long = 0,
    var nome: String,
    var data_nascimeto: String,
    var endereco: String,
    var RG: String,
    var CPF: String
) {

}