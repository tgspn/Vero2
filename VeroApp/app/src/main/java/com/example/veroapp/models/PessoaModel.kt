package com.example.veroapp.models

import android.arch.persistence.room.Entity
import android.arch.persistence.room.PrimaryKey
import java.util.*

@Entity
class PessoaModel(
    @PrimaryKey(autoGenerate = true)
    val id: Long = 0,
    var nome: String,
    var data_nascimeto: String,
    var email:String,
    var RG: String,
    var CPF: String,
    var rua: String,
    var bairro: String,
    var cidade:String,
    var estado:String,
    var pais:String,
    var numero:String,
    var telefone:String
) {
    val endereco: String
        get()= "$rua, $numero, $bairro, $cidade, $estado, $pais"
}