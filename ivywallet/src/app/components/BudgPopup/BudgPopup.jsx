"use client";     
import  { useEffect } from "react"; 
import styles from "./budgPopup.module.css";
import { useState } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { Calendar } from "lucide-react";
import { useRouter } from "next/navigation";
import Cookies from "js-cookie";


const BudgPopup = ({ onClose }) => {
            const [category,setCategory] = useState([]);
            const [title,setTitle] = useState("");
            const [cat,setCat] = useState(0);
            const [selectedDate, setSelectedDate] = useState(new Date());
            const [amount,setAmount] = useState(0);
            const [isOpen, setIsOpen] = useState(false);
            
            const formattedDate = selectedDate.toLocaleString("en-US", {
                  month: "short",
                  day: "numeric",
                  hour: "numeric",
                  minute: "2-digit",
                  hour12: true,
                  }
            );

            const getCategory = async () => {
                        try {
                              const token = Cookies.get("jwtToken");
                              const res = await fetch(`http://localhost:8000/api/Home/GetCategory`, {
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


            const addBudget = async () => {
                  const user = parseInt(Cookies.get("user")); 
                  if (!user) 
                        return alert("User not found");

                  if (!cat) 
                        return alert("Please select a category");

                  const budgetData = {
                        title: title,
                        amount: parseFloat(amount),
                        date: selectedDate.toISOString(),
                        userId: user,
                        categoryIds: [parseInt(cat)]
                  };

                  try {
                  const res = await fetch(`http://localhost:8000/api/Home/AddBudget`, {     
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify(budgetData)
                  });

                  if (res.ok) {
                        onClose();
                  } else {
                        const errorText = await res.text();
                        console.error("Error adding budget:", res.status, errorText);
                        alert("Failed to add budget: " + errorText);
                  }
                  } catch (err) {
                  console.error("Error adding budget:", err);
                  alert("Error adding budget");
                  }
            };



  useEffect(() => {
            getCategory();
      }, []);
           

  return (
            <div className={styles.overlay}>
                  <div className={styles.content}>
                        
                  <h2 className={styles.title}> Add your Budget</h2>

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

                        <div className={styles.popup_buttons}>
                              <button className={styles.btn_cancel} onClick={onClose} >
                                    Cancel
                              </button>
                              <button onClick={addBudget} className={styles.btn_save} >
                                    Add
                              </button>
                        </div>
                  </div>  
            </div>
      );
};
export default BudgPopup;
