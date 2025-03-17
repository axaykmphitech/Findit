using System;
using UnityEngine;
using TMPro;
using System.Globalization;

namespace Bitsplash.DatePicker.Tutorials
{
    public class SelectionTutorial : MonoBehaviour
    {
        public DatePickerSettings DatePicker;
        public TextMeshProUGUI InfoText;

        public static SelectionTutorial Instance;

        public void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (DatePicker != null)
            {
                // handle selection change using a unity event
                DatePicker.Content.OnSelectionChanged.AddListener(OnSelectionChanged);
                //DatePicker.Content.OnDisplayChanged.AddListener(OnDisplayChanged);
                //ShowAllSelectedDates();// show all the selected days in the begining
                //DatePicker.Content.SetMarkerColor(DateTime.Now, Color.red);
            }

        }
        public void OnDisplayChanged()
        {
            var cell = DatePicker.Content.GetCellObjectByDate(DateTime.Now);
            Debug.Log(cell);
            if (cell != null)
            {
                cell.CellEnabled = false;
            }
        }
        public void SelectSingleDate()
        {
            if (DatePicker != null)
            {
                // this method clears the selection and selects the specified date
                DatePicker.Content.Selection.SelectOne(DateTime.Today);
            }
        }
        public void SelectDateRange()
        {
            if (DatePicker != null)
            {
                // this method clears the selection ans selects a spcified range
                //DatePicker.Content.Selection.SelectRange(DateTime.Today, DateTime.Today + TimeSpan.FromDays(5));
            }
        }

        void ShowAllSelectedDates()
        {
            if (InfoText != null)
            {
                string text = "";
                var selection = DatePicker.Content.Selection;

                for (int i = 0; i < selection.Count; i++)
                {
                    var date = selection.GetItem(i);
                    text += date.ToShortDateString();
                }

                try
                {
                    Debug.Log("TEXT::  " + text);
                    DateTime parsedDate = DateTime.Today;
                    parsedDate = DateTime.ParseExact(text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    string formattedDate = parsedDate.ToString("yyyy-MM-dd");

                    InfoText.text = formattedDate;
                    InfoText.GetComponent<TextMeshProUGUI>().color = Color.black;
                }
                catch(Exception)
                {

                }
            }
        }
        void OnSelectionChanged()
        {
            ShowAllSelectedDates();
        }
        // Update is called once per frame
        void Update()
        {
        }
    }
}