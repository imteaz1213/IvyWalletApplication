"use client"
import React, { useEffect, useState } from "react"; 
import styles from "./popup.module.css"
import { Calendar } from "lucide-react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import Cookies from "js-cookie";
import { useRouter } from "next/navigation";

const Popup = ({onClose}) => { 

      const router = useRouter();
      const [category,setCategory] = useState([]);
      const [title,setTitle] = useState("");
      const [cat,setCat] = useState(0);
      const [desc,setDesc] = useState("");
      const [selectedDate, setSelectedDate] = useState(new Date());
      const [isOpen, setIsOpen] = useState(false);
      const [amount,setAmount] = useState(0);
      const [accounts,setAccounts] = useState([]);
      const [accountId,setAccountId] = useState(0);
      

      const getCategory = async () => {
            try {
                  const token = Cookies.get("jwtToken");
                 

                  const res = await fetch("http://localhost:8000/api/Home/GetCategory", {
                        method: "GET",
                        headers: 
                        {
                              Authorization: `Bearer ${token}`,
                              "Content-Type": "application/json"
                        }
                        }
                  );

                  const data = await res.json();
                  setCategory(data);

            } catch (err) {
                  console.error("Error fetching category:", err);
            }
      };

      const getAccount = async () => {
            try 
            {
                  const res = await fetch("http://localhost:8000/api/Home/GetAccount", 
                        {
                              method: "GET",
                              headers: { "Content-Type": "application/json" }
                        }
                  );
                  const data = await res.json();
                  setAccounts(data);
            } 
            catch (err) 
            {
                  console.error("Error fetching account:", err);
            }
      };

      const save = async () => { 
            const user = Cookies.get("user");

                  if (!user) 
                  {
                        router.push("/Login");
                        return;
                  }

            
            const data = {
                  title: title,
                  description: desc,
                  date: selectedDate ? new Date(selectedDate).toISOString() : null,
                  amount: amount,
                  categoryId:cat,
                  userId:user,
                  accountId:accountId
            }
            const response = await fetch("http://localhost:8000/api/Home/AddIncome", 
                  {
                  method: "POST",
                  headers: { "Content-Type": "application/json" },
                  body: JSON.stringify(data)
                  }
            );
            if (!response.ok) 
            {
                  console.error("Failed to save data");
            }
            router.refresh();
            onClose();
      };

      const formattedDate = selectedDate.toLocaleString("en-US", {
            month: "short",
            day: "numeric",
            hour: "numeric",
            minute: "2-digit",
            hour12: true,
            }
      );


      useEffect(
            () => {
                  getCategory()
                  getAccount()
            }
            ,[]);
      
      return (
            <div className={styles.overlay}>
                  <div className={styles.content}>
                        
                  <h2 className={styles.title}> Add your credentials</h2>

                  <div className={styles.form_group}>
                        <input placeholder="Add Title" value={title} onChange={(e)=>setTitle(e.target.value)} type="text" />
                  </div>

                  <div className={styles.form_group}>
                        <input placeholder="Add Amount" value={amount} onChange={(e)=>setAmount(e.target.value)} type="number"/>
                  </div> 

                  <div className={styles.dropdown_container}>
                        <select value={cat} onChange={(e)=>setCat(e.target.value)} className={styles.dropdown_select}>
                              <option value="">Choose Category...</option>
                              {category.map((c) => (
                                    <option key={c.id} value={c.id}>{c.name}</option>
                              ))}
                        </select>                
                  </div>
                  <div className={styles.dropdown_container}>
                        <select value={accountId} onChange={(e)=>setAccountId(e.target.value)} className={styles.dropdown_select}>
                              <option value="">Add Account</option>
                              {accounts.map((a) => (
                                    <option key={a.id} value={a.id}>{a.name}</option>
                              ))}
                        </select>                
                  </div>

                  <div className={styles.info_card} onClick={() => setIsOpen(true)}>
                              <div className={styles.info_item}>
                              <Calendar className={styles.info_icon} size={18} />
                              <span className={styles.info_label}>Created on</span>
                              </div>

                              <span className={styles.info_value}>{formattedDate}</span>

                              {isOpen && (
                              <DatePicker
                              selected={selectedDate}
                              onChange={(date) => {
                                    setSelectedDate(date);
                                    setIsOpen(false); 
                              }}
                              onClickOutside={() => setIsOpen(false)}
                              inline
                              showTimeSelect
                              timeFormat="hh:mm aa"
                              dateFormat="MMM d, yyyy h:mm aa"
                              className={styles.custom_datepicker}
                              />
                              )}
                        </div>
                        
                  <div className={styles.form_group_des}>
                        <textarea placeholder="Add Description"  value={desc} onChange={(e)=>setDesc(e.target.value)} type="text">
                        </textarea> 
                  </div> 
                  
                  

                        <div className={styles.popup_buttons}>
                              <button className={styles.btn_cancel} onClick={onClose} >
                                    Cancel
                              </button>
                              <button onClick={save} className={styles.btn_save} >
                                    Add
                              </button>
                        </div>
                  </div> 
            </div>
      ); 
};

export default Popup;

