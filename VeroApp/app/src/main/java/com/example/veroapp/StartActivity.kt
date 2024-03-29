package com.example.veroapp

import android.app.Activity
import android.content.Intent
import android.os.Bundle
import android.os.Environment
import android.support.design.widget.Snackbar
import android.util.Log
import kotlinx.android.synthetic.main.activity_start.*
import java.io.File
import java.net.HttpURLConnection
import java.net.URL
import java.util.zip.ZipEntry
import java.util.zip.ZipFile
import kotlin.concurrent.thread

class StartActivity : Activity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_start)

        btnCreateWallet.setOnClickListener {
            thread {
                try {

                    getWallet();
                    intent = Intent(this, MainActivity::class.java);
                    startActivity(intent);
                } catch (e: Exception) {
                    runOnUiThread {
                        Snackbar.make(it, e.message.toString(), Snackbar.LENGTH_LONG)
                            .setAction("Action", null).show()
                    }
                }
            }
        }
    }

    fun getWallet() {


        val url = getString(R.string.server_endpoint) + "api/wallet"

        val response = khttp.post(url)
        val wallet: String = response.headers!!["w-user"]!!
        val zipFile: ByteArray = response.content

        val fileName = applicationInfo.dataDir + "/" + "hfc-key-store"
        val file = File(fileName)
        if (!file.exists()) {
            file.mkdirs()
        }

        val zipFileName = fileName + "/" + wallet + ".zip"
        var zip = File(zipFileName)
        try {
            zip.setWritable(true)
            zip.setReadable(true)
            zip.createNewFile()
            zip.outputStream().write(zipFile)

            ZipFile(zipFileName).use { zip ->
                zip.entries().asSequence().forEach { entry ->
                    zip.getInputStream(entry).use { input ->
                        File(fileName + "/" +entry.name).outputStream().use { output ->
                            input.copyTo(output)
                        }
                    }
                }
            }
        } catch (e: Exception) {
            throw  e
        } finally {
            zip.delete()
        }
//            val url = URL(getString(R.string.server_endpoint)+" api/wallet")
//            var fileName = ""
//
//
//
//            with(url.openConnection() as HttpURLConnection) {
//                requestMethod = "PUT"  // optional default is GET
//
//                fileName=this.getHeaderField("w-user")
//                inputStream.bufferedReader().copyTo(File(fileName).outputStream().bufferedWriter())
//            }


    }
}

