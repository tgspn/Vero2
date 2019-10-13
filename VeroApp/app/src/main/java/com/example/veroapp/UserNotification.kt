package com.example.veroapp

import android.app.Activity
import android.arch.persistence.room.DatabaseConfiguration
import android.arch.persistence.room.Room
import android.os.AsyncTask
import android.support.v7.app.AppCompatActivity
import android.os.Bundle
import android.text.TextUtils
import com.example.veroapp.Database.AppDatabase
import com.example.veroapp.models.RequestUserModel
import com.fasterxml.jackson.module.kotlin.jacksonObjectMapper
import com.fasterxml.jackson.module.kotlin.readValue
import io.grpc.Channel
import kotlinx.android.synthetic.main.activity_user_notification.*
import org.json.JSONObject
import java.lang.ref.WeakReference
import kotlin.concurrent.thread
import io.grpc.ManagedChannel
import io.grpc.ManagedChannelBuilder
import io.grpc.ManagedChannelProvider
import org.bouncycastle.asn1.x500.style.RFC4519Style.c
import java.io.PrintWriter
import java.io.StringWriter
import java.util.concurrent.TimeUnit


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
            database =AppDatabase.getInstance(this)



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

private class GrpcTask constructor(activity: Activity) : AsyncTask<String, Void, String>() {
    private val activityReference: WeakReference<Activity> = WeakReference(activity)
    private var channel: ManagedChannel? = null

    override fun doInBackground(vararg params: String): String {
        val host = params[0]
        val message = params[1]
        val portStr = params[2]
        val port = if (TextUtils.isEmpty(portStr)) 0 else Integer.valueOf(portStr)
        return try {
            channel = ManagedChannelBuilder.forAddress(host, port).usePlaintext().build()
            channel.toString()



//            val stub = GreeterGrpc.newBlockingStub(channel)
//            val request = HelloRequest.newBuilder().setName(message).build()
//            val reply = stub.sayHello(request)
//            reply.message
        } catch (e: Exception) {
            val sw = StringWriter()
            val pw = PrintWriter(sw)
            e.printStackTrace(pw)
            pw.flush()

            "Failed... : %s".format(sw)
        }

    }

    override fun onPostExecute(result: String) {
        try {
            channel?.shutdown()?.awaitTermination(1, TimeUnit.SECONDS)
        } catch (e: InterruptedException) {
            Thread.currentThread().interrupt()
        }

//        val activity = activityReference.get() ?: return
//        val resultText: TextView = activity.findViewById(R.id.grpc_response_text)
//        val sendButton: Button = activity.findViewById(R.id.send_button)

//        resultText.text = result
//        sendButton.isEnabled = true
    }
}

