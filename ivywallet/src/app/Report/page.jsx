"use client";
import React, { useState, useEffect } from "react";  
import styles from "./report.module.css";  
import { Calendar, ChevronDown } from "lucide-react";
import Cookies from "js-cookie";


const Report = () => { 
    const [selectedMonth, setSelectedMonth] = useState(new Date().getMonth());
    const [isOpen, setIsOpen] = useState(false);
    const [accounts, setAccounts] = useState([]);
    const [category, setCategory] = useState([]);
    const [cat, setCat] = useState("");
    const [type, setType] = useState("");
    const [data, setData] = useState([]);
    const [accountId, setAccountId] = useState("");
    const [amount, setAmount] = useState(""); 


    const months = [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];
 

    const getAccount = async () => {
        try {
            const res = await fetch("http://localhost:8000/api/Home/GetAccount");
            const data = await res.json();
            setAccounts(data);
        } catch (err) {
            console.error("Error fetching account:", err);
        }
    };


    const getCategory = async () => {
        try {
            const token = Cookies.get("jwtToken");
            const res = await fetch("http://localhost:8000/api/Home/GetCategory", {
                headers: { Authorization: `Bearer ${token}` }
            });
            const data = await res.json();
            setCategory(data);
        } catch (err) {
            console.error("Error fetching category:", err);
        }
    };
    const exportCSV = () => {
        if (data.length === 0) {
            alert("No data to export!");
            return;
        }

        const header = Object.keys(data[0]).join(",") + "\n";
        const rows = data.map(row => Object.values(row).join(",")).join("\n");

        const csvContent = header + rows;

        const blob = new Blob([csvContent], { type: "text/csv;charset=utf-8;" });
        const url = URL.createObjectURL(blob);

        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("download", "report.csv");
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    };


    const applyFilter = async () => {
        const user = Cookies.get("user");
        const params = new URLSearchParams({ user, month: selectedMonth + 1 });
        if(accountId) params.append("accountId", accountId);
        if(cat) params.append("categoryId", cat);
        if(type) params.append("type", type);
        if(amount) params.append("amount", amount);

        const res = await fetch(`http://localhost:8000/api/Home/ApplyFilter/ApplyFilter?${params.toString()}`);
        const d = await res.json();
        setData(d);
    };

    useEffect(() => { 
        getAccount();
        getCategory();
    }, []);
    

    return (
        <div className={styles.container}>
            <div className={styles.filter}>
                <div className={styles.bytype}>
                    <h2 className={styles.section_title}>By Type</h2>
                    <div className={styles.radio_group}>
                        <div className={styles.radio_option}>
                            <input 
                                value="income" 
                                type="radio"  
                                name="type" 
                                checked={type === "income"}
                                onChange={(e) => setType(e.target.value)}
                            />
                            <label htmlFor="incomes">Incomes</label>
                        </div>
                        <div className={styles.radio_option}>
                            <input 
                                value="expense"
                                type="radio" 
                                name="type" 
                                checked={type === "expense"}
                                onChange={(e) => setType(e.target.value)}
                            />
                            <label htmlFor="expenses">Expenses</label>
                        </div>
                    </div>
                </div>

                <div className={styles.bydate}>
                    <h2 className={styles.section_title}>By Date</h2>
                    <div className={styles.monthSelectorWrapper}>
                        <div className={styles.monthButton} onClick={() => setIsOpen(!isOpen)}>
                            <Calendar size={28} />
                            <span>{months[selectedMonth]}</span>
                            <ChevronDown size={16} />
                        </div>
                        {isOpen && (
                            <div className={styles.dropdown}>
                                {months.map((month, index) => (
                                    <div
                                        key={month}
                                        className={styles.dropdownItem}
                                        onClick={() => { setSelectedMonth(index); setIsOpen(false); }}
                                    >
                                        {month}
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>

                    <div className={styles.account}>
                        <h2 className={styles.section_title}>By Account</h2>
                        <select value={accountId} onChange={(e) => setAccountId(e.target.value)} className={styles.dropdown_select}>
                            <option value="">Choose Account...</option>
                            {accounts.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}
                        </select>       
                    </div>

                    <div className={styles.category}>
                        <h2 className={styles.section_title}>By Category</h2>
                        <select value={cat} onChange={(e) => setCat(e.target.value)} className={styles.dropdown_select}>
                            <option value="">Choose Category...</option>
                            {category.map((c) => <option key={c.id} value={c.id}>{c.name}</option>)}
                        </select>   
                    </div>

                    <div className={styles.amount}>
                        <h2 className={styles.section_title}>Amount</h2>
                        <input 
                            type="number" 
                            value={amount} 
                            onChange={(e) => setAmount(e.target.value === "" ? "" : Number(e.target.value))}
                            className={styles.dropdown_select} 
                            placeholder="Enter Amount"
                        />
                    </div>

                    <div className={styles.submit}>
                        <button className={styles.dropdown_select} onClick={applyFilter}>Apply Filter</button>
                        <button className={styles.dropdown_select} onClick={exportCSV}>Export</button>
                    </div>
                </div>
            </div>

            <div className={styles.report}>
                  {data.length === 0 ? (
                        <h3>Please apply filters to see the report</h3>
                  ) : (
                     <div className={styles.cards}>
                        {data.map((item, index) => (
                              <div key={index} className={styles.card}>
                                    <div className={styles.card_title}>
                                          <h2>{item.type}</h2>
                                          <p>{item.categoryName}</p>
                                    </div>
                                    <div className={styles.card_amt}>
                                          <h1>{item.amount}</h1>
                                    </div>
                              </div>
                        ))}
                        </div>
                  )}
                  </div>
            </div>
    );
};

export default Report;
