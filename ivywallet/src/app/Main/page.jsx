
"use client"
import React from "react";
import styles from "./main.module.css"
import { useState } from "react";
import { Calendar, ChevronDown } from "lucide-react";
import { useRouter } from "next/navigation";
import Cookies from "js-cookie";
import { useEffect } from "react";

const Main = () => {
      const [userIncomeData, setUserIncomeData] = useState([]);
      const [userExpenseData, setUserExpenseData] = useState([]);
      const [selectedMonth, setSelectedMonth] = useState(new Date().getMonth());
      const [isOpen, setIsOpen] = useState(false);
      const router = useRouter();
      const [inc, setInc] = useState(0);
      const [exp, setExp] = useState(0);
      const [userCurr,setUserCurr] = useState("");
      const [total,setTotal] = useState(0);


      const months = [
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
      ];
      
      const getIncomeDataByMonth = async (month) => {
            const user_id = Cookies.get("user");
            const res = await fetch(`http://localhost:8000/api/Home/GetIncomeByMonth/${user_id}/${month + 1}`,
                  {
                        method: "GET",
                        headers: { "Content-Type": "application/json" }
                  }
            );
            const data = await res.json();
            setUserIncomeData(data);
      };


      const getExpenseDataByMonth = async (month) => {
            const user_id = Cookies.get("user");
            const res = await fetch(`http://localhost:8000/api/Home/GetExpenseByMonth/${user_id}/${month + 1}`,   
                  {
                        method: "GET",                                  
                        headers: { "Content-Type": "application/json" }             

                  }
            );
            const data = await res.json();
            setUserExpenseData(data);
      };

      
      const getInc = async (month) => {
            const user_id = Cookies.get("user");
            const res = await fetch(`http://localhost:8000/api/Home/FullIncomeOfAUser/${user_id}/${month + 1}`,
                  {
                        method: "GET",                                  
                        headers: { "Content-Type": "application/json" }
                  }
            );
            const inc = await res.json();          
            setInc(inc);
      };

      const getExp = async (month) => {
            const user_id = Cookies.get("user");
            const res = await fetch(`http://localhost:8000/api/Home/FullExpenseOfAUser/${user_id}/${month + 1}`,
                  {
                        method: "GET",                                  
                        headers: { "Content-Type": "application/json" }
                  }
            );
            const exp = await res.json();
            setExp(exp);
      };

      const getUserCurrency = async () => {
            const user_id = Cookies.get("user");
            const res = await fetch(`http://localhost:8000/api/Home/GetUserCurrency/${user_id}`,
                  {
                        method: "GET",                                  
                        headers: { "Content-Type": "application/json" }
                  }
            );
            const data = await res.json();
            setUserCurr(data[0].currency);
      }


      const logout = () => {
            Cookies.remove("user");
            Cookies.remove("jwtToken");
            router.push("/Login");
      }
      const getBalance = () => {
            const total = inc - exp;
            setTotal(total);
      }

      useEffect(() => { 
            getUserCurrency();
      }
      ,[]);

      useEffect(() => {
            setTotal(inc - exp);
      }, [inc, exp]);


    
      useEffect(() => {
            getInc(selectedMonth);
            getExp(selectedMonth);
      }, [selectedMonth]);     

      useEffect(() => {
            getIncomeDataByMonth(selectedMonth);
            getExpenseDataByMonth(selectedMonth);
      }, [selectedMonth]);


      return (
            <div className={styles.container}>
                  <div className={styles.nav_drawer}>
                        <h1>Hi</h1>
                        <div className={styles.items}>
                              <div onClick={() => router.push("/Income")} className={styles.item}>
                                    <img src="/download.png" />
                                    <span>Income</span>
                              </div>
                              <div onClick={() => router.push("/Expense")} className={styles.item}>
                                    <img src="/upload.png" />
                                    <span>Expense</span>
                              </div>

                              <div onClick={() => router.push("/Budget")} className={styles.item}>
                                    <img src="/money-bag.png" />
                                    <span>Budget</span>
                              </div>
                              <div onClick={() => router.push("/Report")} className={styles.item}>
                                    <img src="/report.png" />
                                    <span>Report</span>
                              </div>
                              <div onClick={logout} className={styles.item}>
                                    <img src="/logout.png" />
                                    <span>Logout</span>
                              </div>
                        </div>
                  </div>
                  <div>
                        <div className={styles.nav}>
                              <div></div>
                              <div className={styles.monthSelectorWrapper}>
                                    <div className={styles.monthButton} onClick={() => setIsOpen(!isOpen)}>
                                          <Calendar size={28} />
                                          <span>{months[selectedMonth]}</span>
                                          <ChevronDown size={16} />
                                    </div>

                                    {isOpen &&
                                          (
                                                <div className={styles.dropdown}>
                                                      {months.map((month, index) => (
                                                            <div
                                                                  key={month}
                                                                  className={styles.dropdownItem}
                                                                  onClick={() => {
                                                                        setSelectedMonth(index);
                                                                        setIsOpen(false);
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
                        </div>
                        <div className={styles.inc_exp}>
                              <div onClick={() => router.push("/Income")} className={styles.inc}>
                                    <div className={styles.title}>
                                          <img src="/download.png" className={styles.img} />
                                          <b>Income</b>
                                    </div>
                                    <div className={styles.mony}>
                                          <h1>{inc}</h1>
                                          <h1>{userCurr}</h1>
                                    </div>
                              </div>
                              <div onClick={() => router.push("/Expense")} className={styles.exp}>
                                    <div className={styles.title}>
                                          <img src="/upload.png" className={styles.img} />
                                          <b>Expense</b>
                                    </div>
                                    <div className={styles.mony}>
                                          <h1>{exp}</h1>
                                          <h1>{userCurr}</h1>
                                    </div>
                              </div>
                        </div> 
                        <div className={styles.total}>
                              <h1>Current Balance : {total}</h1>
                        </div>
                        {userIncomeData.map((d) => (
                              <div key={d.c_Name} className={styles.card}>
                                    <img src="/download.png" className={styles.img} />
                                    <div className={styles.cat} style={{ background: d.c_Color }}>
                                          <img src={d.c_Image} className={styles.icon} />
                                          <span>{d.c_Name}</span>
                                    </div>
                                    <div className={styles.acc}>
                                          <img src="/dollar.png" className={styles.icon} />
                                          <span>{d.a_Name}</span>
                                    </div>
                                    <h1>{d.i_Amount}  {d.cr_Currency}</h1>
                              </div>
                        ))}

                        { userExpenseData.map((d) => (
                              <div key={d.c_Name} className={styles.card}>
                                    <img src="/upload.png" className={styles.img} />
                                    <div className={styles.cat} style={{ background: d.c_Color }}>
                                          <img src={d.c_Image} className={styles.icon} />
                                          <span>{d.c_Name}</span>
                                    </div>
                                    <div className={styles.acc}>
                                          <img src="/dollar.png" className={styles.icon} />
                                          <span>{d.a_Name}</span>
                                    </div>
                                    <h1>{d.e_Amount}  {d.cr_Currency}</h1>
                              </div>
                        ))}
                        {userIncomeData.length === 0 && userExpenseData.length === 0 && (<h1 className={styles.n}>No data found</h1>)}
                  </div>
            </div>
      );
};
export default Main;


