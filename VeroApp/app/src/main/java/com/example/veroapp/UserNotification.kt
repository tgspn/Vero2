package com.example.veroapp

import android.arch.persistence.room.DatabaseConfiguration
import android.arch.persistence.room.Room
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.text.TextUtils
import com.example.veroapp.Database.AppDatabase
import com.example.veroapp.models.RequestUserModel
import com.fasterxml.jackson.module.kotlin.jacksonObjectMapper
import com.fasterxml.jackson.module.kotlin.readValue
import kotlinx.android.synthetic.main.activity_user_notification.*
import org.json.JSONObject
import kotlin.concurrent.thread

class UserNotification : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_user_notification)

        var modelText = intent.getStringExtra("modelText")
        val mapper = jacksonObjectMapper()
        val model = mapper.readValue<RequestUserModel>(modelText)

        txtResult.text = "\t" + model.storeName + "\r\n\r\nCampos:\r\n\r\n" + TextUtils.join("\r\n", model.fields)


        thread {
            var database: AppDatabase
            database = Room.databaseBuilder(
                this,
                AppDatabase::class.java,
                "vero-database"
            ).fallbackToDestructiveMigration()
                .build()



            run {
                btnConfirmar.setOnClickListener {
                    thread {
                        var resp = HashMap<String, String>()
                        var pessoaDao = database.pessoaDAO()
                        model.fields.forEach {
                            try {
                                var field = it
                                var value = pessoaDao.get()
                                if (field == "endereco")
                                    resp[field] = value.endereco
                                if (field == "nome")
                                    resp[field] = value.nome
                            } catch (ex: Exception) {
                                ex.printStackTrace()
                            }
                        }
                        model.response = resp
                        val url = getString(R.string.server_endpoint) + "api/info/" + model.id
                        try {
                            var header = HashMap<String, String>()
                            header["Content-Type"] = "application/json"
                            var json = mapper.writeValueAsBytes(model)
                            var result = khttp.post(url, data = json, headers = header)
                            var content = result.text
                        } catch (ex: Exception) {
                            ex.printStackTrace()
                        }
                        this.finishActivity(0)
                    }
                }
            }
        }
        btnCancelar.setOnClickListener {
            this.finishActivity(0);
        }
    }
}
