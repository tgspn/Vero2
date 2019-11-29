package com.example.veroapp

import android.arch.persistence.room.DatabaseConfiguration
import android.arch.persistence.room.Room
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.support.v7.widget.LinearLayoutManager
import android.text.TextUtils
import com.example.veroapp.Database.AppDatabase
import com.example.veroapp.adpters.FieldsAdapter
import com.example.veroapp.models.RequestUserModel
import com.fasterxml.jackson.core.type.TypeReference
import com.fasterxml.jackson.module.kotlin.convertValue
import com.fasterxml.jackson.module.kotlin.jacksonObjectMapper
import com.fasterxml.jackson.module.kotlin.readValue
import kotlinx.android.synthetic.main.activity_user_notification.*
import org.json.JSONObject
import kotlin.concurrent.thread
import kotlin.reflect.jvm.internal.impl.resolve.scopes.MemberScope

class UserNotification : AppCompatActivity() {
    val database: AppDatabase = AppDatabase.getInstance(this)
    var list = mutableListOf<FieldsModel>()
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_user_notification)

        var modelText = intent.getStringExtra("modelText")
        val mapper = jacksonObjectMapper()
        val model = mapper.readValue<RequestUserModel>(modelText)

        txtStoreName.text = model.storeName
        txtValor.text=model.value.toString( )
        model.fields.forEach {
            list.add(FieldsModel(it, true))
        }
        lvFields.layoutManager = LinearLayoutManager(this)
        lvFields.adapter = FieldsAdapter(this, list)
//        txtResult.text = "\t" + model.storeName + "\r\n\r\nCampos:\r\n\r\n" + TextUtils.join("\r\n", model.fields)

//        thread {
//            database = AppDatabase.getInstance(this)
//        } 


        btnConfirmar.setOnClickListener {
            thread {
                var resp = HashMap<String, String>()
                var pessoaDao = database.pessoaDAO()
                var walletDao=database.walletDAO()
                list.forEach {
                    try {
                        if (it.checked) {
                            var field = it.fieldName
                            var value = pessoaDao.get()
//                            //testar - não funcionou
//                            resp[field] = pessoaDao.getFieldValue(field)

                            if (field == "endereco")
                                resp[field] = value.endereco
                            else if(field=="rua")
                                resp[field] = value.rua
                            else if(field=="cidade")
                                resp[field] = value.cidade
                            else if(field=="bairro")
                                resp[field] = value.bairro
                            else if(field=="estado")
                                resp[field] = value.estado
                            else if(field=="pais")
                                resp[field] = value.pais
                            else if(field=="numero")
                                resp[field] = value.numero
                            else if(field=="cpf")
                                resp[field] = value.CPF
                            else if(field=="rg")
                                resp[field] = value.RG
                            else if(field=="data_nascimeto")
                                resp[field] = value.data_nascimeto
                            else if(field=="email")
                                resp[field] = value.email
                            else if (field == "nome")
                                resp[field] = value.nome
                            else if(field=="telefone")
                                resp[field] = value.telefone
                        }
                    } catch (ex: Exception) {
                        ex.printStackTrace()
                    }
                }
                model.response = resp
                val url = getString(R.string.server_endpoint) + "api/info/" + walletDao.get().wallet
                try {
                    var header = HashMap<String, String>()
                    header["Content-Type"] = "application/json"
                    var json = mapper.convertValue<Map<String, Any>>(model)//mapper.writeValueAsBytes(model)
                    var result = khttp.post(url, json = json, headers = header)
                    var content = result.text
                    if (content == "") {

                    }
                } catch (ex: Exception) {
                    ex.printStackTrace()
                }
                this.finish()
            }
        }

        btnCancelar.setOnClickListener {
            thread {
                //                var database: AppDatabase
//                database = AppDatabase.getInstance(this)
                var pessoaDao = database.pessoaDAO()
                val url = getString(R.string.server_endpoint) + "api/info/" + model.id
                try {
                    var header = HashMap<String, String>()
                    header["Content-Type"] = "application/json"
                    var json = mapper.convertValue<Map<String, Any>>(model)//mapper.writeValueAsBytes(model)
                    var result = khttp.post(url, json = json, headers = header)
                    var content = result.text
                    if (content == "") {

                    }
                } catch (ex: Exception) {
                    ex.printStackTrace()
                }
                this.finish();
            }

        }
    }
}
