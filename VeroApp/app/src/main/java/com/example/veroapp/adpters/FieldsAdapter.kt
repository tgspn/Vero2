package com.example.veroapp.adpters

import android.content.Context
import android.support.v7.widget.RecyclerView
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import com.example.veroapp.FieldsModel
import com.example.veroapp.R
import com.example.veroapp.models.KeyModel
import kotlinx.android.synthetic.main.content_fields_item.view.*
import kotlinx.android.synthetic.main.content_keys_item.view.*
import kotlin.concurrent.thread

class FieldsAdapter(val context: Context, private var list: MutableList<FieldsModel>) : RecyclerView.Adapter<FieldsAdapter.ViewHolder>() {
    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): ViewHolder {
        val view = LayoutInflater.from(context).inflate(R.layout.content_fields_item, parent, false)
        return ViewHolder(view)
    }

    override fun getItemCount(): Int {
        return list.size
    }

    override fun onBindViewHolder(holder: ViewHolder, position: Int) {

            var item = list[position]
      //  holder.bind(item)
        holder.checked=item.checked
        holder.fieldName=item.fieldName
//            holder.field.text = item.fieldName
//            holder.field.isChecked = item.checked

    }

    class ViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        var fieldName
            get() = itemView.swField.text
            set(value) {itemView.swField.text=value}

        var checked
            get() =itemView.swField.isChecked
            set(value){itemView.swField.isChecked=value}

        fun bind(ob: FieldsModel) {
            itemView.swField.text=ob.fieldName
            itemView.swField.isChecked=ob.checked

        }

//        var field = itemView.swField
//            get()=


    }

}