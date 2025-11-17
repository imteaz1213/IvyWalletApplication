"use client"
import React, { use, useState } from "react"; 
import Popup from "../components/Popup/Popup";
import styles from "./income.module.css"
import { PieChart, Pie, Cell, Tooltip, Legend, ResponsiveContainer } from "recharts";
import { useEffect } from "react";
import Cookies from "js-cookie";
import { Calendar, ChevronDown } from "lucide-react";

const Income = () => { 
      const [open,setOpen] = useState(false);
      const [userIncome, setUserIncome] = useState([]);
      const [selectedMonth, setSelectedMonth] = useState(new Date().getMonth());
      const [isOpen_c, setIsOpen_c] = useState(false);
      const [total, setTotal] = useState(0);  
        
      const months = [
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
      ];    


      const getUserIncome = async (month) => {
            const user_id = Cookies.get("user");
            const res = await fetch(`http://localhost:8000/api/Home/GetIncomeByMonth/${user_id}/${month + 1}`,
                  {
                        method: "GET",
                        headers: { "Content-Type": "application/json" }
                  }
            );
            const data = await res.json();
            setUserIncome(data);
      };

      const getTotalIncome = async (month) => {
            const user_id = Cookies.get("user");
            const res = await fetch(`http://localhost:8000/api/Home/FullIncomeOfAUser/${user_id}/${month + 1}`,
                  {
                        method: "GET",
                        headers: { "Content-Type": "application/json" }
                  }
            );
            const data = await res.json();
            setTotal(data);
      };

      const data = userIncome.map((item) => ({
            name: item.c_Name,
            value: item.i_Amount
      }));
      const COLORS = userIncome.map((item) => item.c_Color);

      useEffect(() => {
            getUserIncome(selectedMonth);
      }, [selectedMonth]);

      useEffect(() => { 
            getTotalIncome(selectedMonth);
      }, [selectedMonth]);

      return (
            <div className={styles.container}>
                  <div className={styles.nav}>
                        <div className={styles.upper}>
                              <h1>Income</h1>
                              <div>{total} BDT</div>
                        </div>
                        <div className={styles.monthSelectorWrapper}>
                                    <div className={styles.monthButton} onClick={() => setIsOpen_c(!isOpen_c)}>
                                          <Calendar size={28} />
                                          <span>{months[selectedMonth]}</span>
                                          <ChevronDown size={16} />
                                    </div> 

                                    {isOpen_c &&
                                          (
                                                <div className={styles.dropdown}>
                                                      {months.map((month, index) => (
                                                            <div
                                                                  key={month}
                                                                  className={styles.dropdownItem}
                                                                  onClick={() => {
                                                                        setSelectedMonth(index);
                                                                        setIsOpen_c(false);
                                                                  }}
                                                            >
                                                                  {month}
                                                            </div>
                                                      )
                                                      )
                                                      }
                                                </div>
                                          )
                                    }
                              </div>
                        <div className={styles.img} onClick={() => setOpen(true)}>
                              <img src="/add.png" height={23} width={23}/>
                        </div>
                  </div>
                  <div className={styles.content}>
                  <div className={styles.pie_chart}>
                        <ResponsiveContainer width="100%" height={500}>
                              <PieChart>
                                    <Pie
                                    data={data}
                                    cx="50%"
                                    cy="50%"
                                    outerRadius={200}
                                    dataKey="value"
                                    label
                                    >
                                    {data.map((entry, index) => (
                                    <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                                    ))}
                                    </Pie>
                                    <Tooltip />
                                    <Legend />
                              </PieChart>
                        </ResponsiveContainer>
                  </div>
                        <div className={styles.card_wrapper}>
                        {userIncome.map((v)=>  (         
                              <div key={v.c_Name} style={{background:v.c_Color}} className={styles.cards}>
                                    <div className={styles.img}>
                                          <img src={v.c_Image}  height={33} width={33}/>
                                    </div>
                                    <div className={styles.card_title}>
                                          <span>{v.c_Name}</span>
                                          <h1>{v.i_Amount}</h1>
                                    </div>
                              </div>
                        ))}
                        </div>
                  </div>
                  <div className={styles.h}></div>
                  {open &&
                        <Popup onClose={() => setOpen(false)}/>
                  }
            </div>
      );
};
export default Income;
