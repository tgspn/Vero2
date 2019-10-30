package com.example.veroapp

import android.arch.persistence.room.Room
import android.content.Intent
import android.os.Bundle
import android.support.design.widget.Snackbar
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager
import android.widget.LinearLayout
import android.widget.Toast
import com.example.veroapp.Database.AppDatabase
import com.example.veroapp.adpters.KeysAdapter
import com.example.veroapp.models.KeyModel
import com.fasterxml.jackson.module.kotlin.convertValue
import com.google.zxing.integration.android.IntentIntegrator
import com.google.zxing.integration.android.IntentResult
import com.journeyapps.barcodescanner.SourceData
import kotlinx.android.synthetic.main.activity_key_manager.*
import kotlinx.android.synthetic.main.content_key_manager.*
import com.fasterxml.jackson.module.kotlin.jacksonObjectMapper
import com.fasterxml.jackson.module.kotlin.readValue
import java.util.*
import kotlin.concurrent.thread


class KeyManagerActivity : AppCompatActivity() {
    var scannedResult: String = ""
    val database: AppDatabase = AppDatabase.getInstance(this)
    val mapper = jacksonObjectMapper()
    lateinit var adapter: KeysAdapter
    lateinit var list:MutableList<KeyModel>

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_key_manager)
//        setSupportActionBar(toolbar)
        lvKeys.layoutManager = LinearLayoutManager(this)

        try {
            thread {
                list=database.keyDAO().all().toMutableList()
                adapter = KeysAdapter(this, list)
                run {
                    lvKeys.adapter = adapter
                }
            }


        } catch (e: Exception) {
            Toast.makeText(this, e.message, Toast.LENGTH_LONG)
        }
        fab_take_qrcode.setOnClickListener { view ->

            run {
                var list = mutableListOf("QR Code", "qrcode", "qr code");
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

        if (result != null) {

            if (result.contents != null) {
                scannedResult = result.contents
                //  txtValue.text = this.scannedResult

                try {
                    val model = mapper.readValue<ValidaPcModel>(scannedResult)

                    val keyMode = KeyModel(
                        computerName = model.computerName,
                        date = model.date,
                        ip = model.ip,
                        pc_id = model.id
                    )
                    thread {
                        database.keyDAO().add(keyMode)
                        model.publicKey = keyMode.id
                        val url = getString(R.string.server_endpoint) + "api/validate/"
                        try {
                            var header = HashMap<String, String>()
                            header["Content-Type"] = "application/json"
                            var json = mapper.convertValue<Map<String, Any>>(model)//mapper.writeValueAsBytes(model)
                            var result = khttp.post(url, json = json, headers = header)
                            var content = result.text
                            if (result.statusCode == 200) {
                                
                                adapter.notifyDataSetChanged()
                            }
                        } catch (ex: Exception) {
                            ex.printStackTrace()
                        }
                    }


                } catch (e: Exception) {
                    e.printStackTrace()
                }
            } else {
//                txtValue.text = "scan failed"
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
//            txtValue.text = scannedResult
        }
    }

}
