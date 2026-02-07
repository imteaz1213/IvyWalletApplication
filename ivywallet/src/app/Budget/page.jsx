"use client";
import React, { useEffect, useState } from "react";
import styles from "./budget.module.css";
import BudgPopup from "../components/BudgPopup/BudgPopup";
import { Calendar, ChevronDown } from "lucide-react";
import { useRouter } from "next/navigation";
import Cookies from "js-cookie";

const BudgetPage = () => {
  const [isOpen, setIsOpen] = useState(false);
  const [open_drawer, setOpen_drawer] = useState(false);
  const [selectedMonth, setSelectedMonth] = useState(new Date().getMonth());
  const [budgetData, setBudgetData] = useState([]);
  const router = useRouter();

  const months = [
    "January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
  ];
  
  const getBudget = async (month) => {
    const user = Cookies.get("user");
    const res = await fetch(`http://localhost:8000/api/Home/GetBudgetById/${user}/${month}`,
      {
        method: "GET",
        headers: {
          "Content-Type": "application/json"
        }
      }
    );
    const data = await res.json();
    setBudgetData(data);
  };

  const budgets = budgetData.map((item) => ({
    name: item.budgetName,
    category: item.categoryName,
    total: item.total,
    spent: item.spent,
    color: item.color
  }));

  useEffect(() => {
      getBudget(selectedMonth + 1);
  }, [selectedMonth]);

  return (
    <div className={styles.container}>
      <div className={styles.nav}>
        <h1>Hi</h1>
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
                  onClick={() => {
                    setSelectedMonth(index);
                    setIsOpen(false);
                  }}
                >
                  {month}
                </div>
              ))}
            </div>
          )}
        </div>
        <div className={styles.btn}>
            <img onClick={() => setOpen_drawer(true)} src="/add.png" height={32} width={32} alt="Add"/>
          </div>
      </div>
      
      <div className={styles.budgets}>
        <div className={styles.budget}>
          {budgets.map((budget, index) => {
            const exceeded = budget.spent > budget.total;
            const left = exceeded
              ? budget.spent - budget.total
              : budget.total - budget.spent;
            const percent = Math.min((budget.spent / budget.total) * 100, 100);

            return (
              <div key={index} className={styles.card}>
                <div className={styles.cardHeader}>
                  <div>
                    <h3>{budget.name}</h3>
                    <p className={styles.sub}>{budget.category}</p>
                  </div>
                  <div className={styles.amount}>{budget.total} BDT</div>
                </div>

                <div
                  className={styles.status}
                  style={{
                    backgroundColor: exceeded
                      ? "#ee11118e"
                      : budget.color,
                  }}
                >
                  <p className={styles.statusTitle}>
                    {exceeded ? "Budget exceeded by" : "Left to spend"}
                  </p>

                  <h4>
                    {left.toFixed(2)} <span>BDT</span>
                  </h4>

                  <p className={styles.ratio}>
                    {budget.spent.toFixed(2)}/{budget.total.toFixed(2)} BDT
                  </p>

                  <div className={styles.progressWrapper}>
                    <div
                      className={styles.progressBar}
                      style={{
                        width: `${percent}%`, 
                        backgroundColor: exceeded
                          ? "#b91c1c"
                          : "rgba(255,255,255,0.9)",
                      }}
                    />
                  </div>
                </div>
              </div>
            );
          })}
        </div>
      </div>
      {open_drawer && <BudgPopup onClose={() => setOpen_drawer(false)} />}
    </div>
  );
};
export default BudgetPage;

