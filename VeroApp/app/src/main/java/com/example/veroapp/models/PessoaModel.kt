package com.example.veroapp.models

import android.arch.persistence.room.Entity
import android.arch.persistence.room.PrimaryKey

@Entity
class PessoaModel(
    @PrimaryKey(autoGenerate = true)
    val id: Long = 0,
    val nome: String,
    val data_nascimeto: String,
    val endereco: String,
    val RG: String,
    val CPF: String
) {

}