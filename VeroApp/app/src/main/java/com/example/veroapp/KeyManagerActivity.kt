package com.example.veroapp

import android.arch.persistence.room.Room
import android.content.Intent
import android.os.Bundle
import android.support.design.widget.Snackbar
import android.support.v7.app.AppCompatActivity;
import android.widget.Toast
import com.example.veroapp.Database.AppDatabase
import com.example.veroapp.models.KeyModel
import com.google.zxing.integration.android.IntentIntegrator
import com.google.zxing.integration.android.IntentResult
import com.journeyapps.barcodescanner.SourceData
import kotlinx.android.synthetic.main.activity_key_manager.*
import kotlinx.android.synthetic.main.content_key_manager.*
import com.fasterxml.jackson.module.kotlin.jacksonObjectMapper
import com.fasterxml.jackson.module.kotlin.readValue
import kotlin.concurrent.thread


class KeyManagerActivity : AppCompatActivity() {
    var scannedResult: String = ""
    private lateinit var database: AppDatabase

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_key_manager)
        setSupportActionBar(toolbar)
        try {
            thread {
                database = Room.databaseBuilder(
                    this,
                    AppDatabase::class.java,
                    "vero-database"
                ).fallbackToDestructiveMigration()
                    .build()

                run {
                    database.keyDAO().all().forEach {
                        txtValue.text = "Computador: " + it.computerName + "\r\n" +
                                "data: " + it.date + "\r\n" +
                                "ip: " + it.ip + "\r\n" +
                                "id: " + it.id + "\r\n" +
                                "PC id: " + it.pc_id
                    }
                }
            }

        }catch (e:Exception){
            Toast.makeText(this, e.message,Toast.LENGTH_LONG)
        }
        fab_take_qrcode.setOnClickListener { view ->

            run {
                var list= mutableListOf("QR Code","qrcode","qr code");
                IntentIntegrator(this@KeyManagerActivity)
                    .setDesiredBarcodeFormats(list)
                    .setDesiredBarcodeFormats(IntentIntegrator.QR_CODE)
                    .initiateScan(list);
            }
        }

//        txtValue.freezesText=true
        supportActionBar?.setDisplayHomeAsUpEnabled(true)
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {

        var result: IntentResult? = IntentIntegrator.parseActivityResult(requestCode, resultCode, data)

        if(result != null){

            if(result.contents != null){
                scannedResult = result.contents
                txtValue.text = this.scannedResult
                val mapper = jacksonObjectMapper()
                try {
                    val model = mapper.readValue<ValidaPcModel>(scannedResult)

                    txtValue.text = model.computerName + "\r\n" +
                        model.date+"\r\n"+
                            model.ip + "\r\n" +
                            model.id

                    val keyMode = KeyModel(
                        computerName = model.computerName,
                        date = model.date,
                        ip = model.ip,
                        pc_id = model.id
                    )
                    thread {
                        database.keyDAO().add(keyMode)
                    }
                }catch (e: Exception){
                    e.printStackTrace()
                }
            } else {
                txtValue.text = "scan failed"
            }
        } else {
            super.onActivityResult(requestCode, resultCode, data)
        }
    }

    override fun onSaveInstanceState(outState: Bundle?) {

        outState?.putString("scannedResult", scannedResult)
        super.onSaveInstanceState(outState)
    }

    override fun onRestoreInstanceState(savedInstanceState: Bundle?) {
        super.onRestoreInstanceState(savedInstanceState)

        savedInstanceState?.let {
            scannedResult = it.getString("scannedResult")
            txtValue.text = scannedResult
        }
    }

}
