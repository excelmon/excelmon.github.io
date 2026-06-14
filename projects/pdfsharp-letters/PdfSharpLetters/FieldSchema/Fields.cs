using PdfSharpLetters.Sections;

namespace PdfSharpLetters.FieldSchema
{
    public static class Fields
    {
        // ===== Salesforce Field and Object API =====
        // The SF Object: "Letter__c"
        public static TextField LTRName = new("Letter", "Name", "LTR-1234567");
        // Parent Object: "Garnishment_Order__c"
        public static TextField GARName = new("Garnishment Order Name", "Garnishment_Order__r.Name", "GAR-00012345");

        // Letter__c fields
        public static TextField CustomerBillingId = new("Customer Billing Id", "Customer_Billing_ID__c", "C000598");
        public static TextField LetterType = new("Letter Type", "Letter_Type__c", "DEMO_LETTER");
        public static TextField GarnishmentCaseNumber = new("Garnishment Case Number", "Garnishment_Number__c", "05-CIV-2026-001");

        private static readonly DateOnly _today = DateOnly.FromDateTime(DateTime.Now);
        public static DateField LetterProcessedDate = new("Letter Processed Date", "Letter_Processed_Date__c", _today.AddDays(-1));

        public static TextField EmployeeFirstName = new("Employee First Name", "Employee_First_Name__c", "Carl");
        public static TextField EmployeeLastName = new("Employee Last Name", "Employee_Last_Name__c", "Sagan");
        public static readonly string EmployeeFullName = EmployeeFirstName.Value + " " + EmployeeLastName.Value;

        public static NumberField ScheduledWeeklyHours = new("Scheduled Weekly Hours", "Scheduled_Weekly_Hours__c", 40);
        public static NumberField GrossEarnings = new("Gross Amount", "Gross_Amount__c", 15000.00);
        public static NumberField FederalIncomeTax = new("Federal Income Tax", "Federal_Income_Tax__c", 10000.99);
        public static NumberField SocialSecurityTax = new("Social Security Tax", "Social_Security_Tax__c", 3333.00);
        public static NumberField MedicareTax = new("Medicare Tax", "Medicare_Tax__c", 500.73);
        private static readonly double _totalTax = FederalIncomeTax.Value + SocialSecurityTax.Value + MedicareTax.Value;
        public static NumberField TotalTaxes = new("Total Taxes", "Total_Taxes__c", _totalTax);
        private static readonly double _disposableEarnings = GrossEarnings.Value - TotalTaxes.Value;
        public static NumberField DisposableEarnings = new("Disposable Earnings", "Disposable_Earnings__c", _disposableEarnings);
        // Dummy estimate: 25% of disposable earnings as a placeholder withholding amount
        public static NumberField EstimatedWithholding = new("Estimated Withholding", "Estimated_Withholding__c", _disposableEarnings * .25);

        public static NumberField ProcessingFee = new("Processing Fee", "Processing_Fee__c", 0.99);

        // The Letter Recipient
        private static TextField _mailingRecipientName = new("Mailing Recipient Name", "Mailing_Recipient_Name__c", "Collections Services Co.");
        private static TextField _mailingRecipientAddressLine1 = new("Mailing Recipient Address Line 1", "Mailing_Recipient_Address_Line_1__c", "12345 Collections Dr.");
        private static TextField _mailingRecipientAddressLine2 = new("Mailing Recipient Address Line 2", "Mailing_Recipient_Address_Line_2__c", "Unit C");
        private static TextField _mailingRecipientCity = new("Mailing Recipient City", "Mailing_Recipient_City__c", "Pensacola");
        private static TextField _mailingRecipientState = new("Mailing Recipient State", "Mailing_Recipient_State__c", "Florida");
        private static TextField _mailingRecipientZipcode = new("Mailing Recipient Zipcode", "Mailing_Recipient_Zipcode__c", "32501");

        public static Address MailingRecipientAddress = new(
            _mailingRecipientName.Value!,
            _mailingRecipientAddressLine1.Value!,
            _mailingRecipientAddressLine2.Value,
            _mailingRecipientCity.Value!,
            _mailingRecipientState.Value!,
            _mailingRecipientZipcode.Value!);

        // Employee Address
        private static TextField _employeeAddressLine1 = new("Employee Address Line 1", "Employee_Address_Line_1__c", "333 Zeta Reticuli Blvd.");
        private static TextField _employeeAddressLine2 = new("Employee Address Line 2", "Employee_Address_Line_2__c", "Building A, Apt #19");
        private static TextField _employeeCity = new("Employee City", "Employee_City__c", "Gulf Breeze");
        private static TextField _employeeState = new("Employee State", "Employee_State__c", "Florida");
        private static TextField _employeeZipcode = new("Employee Zipcode", "Employee_Zipcode__c", "32561");

        public static Address EmployeeAddress = new(
            EmployeeFullName,
            _employeeAddressLine1.Value!,
            _employeeAddressLine2.Value,
            _employeeCity.Value!,
            _employeeState.Value!,
            _employeeZipcode.Value!);

        // Customer Address
        private static TextField _customerAccountName = new("Company", "Company__c", "NASA");
        private static TextField _customerAddressLine1 = new("Company Address Line 1", "Company_Address_Line_1__c", "300 Hidden Figures Way SW");
        private static TextField _customerAddressLine2 = new("Company Address Line 2", "Company_Address_Line_2__c", null);
        private static TextField _customerCity = new("Company City", "Company_City__c", "Washington");
        private static TextField _customerState = new("Company State", "Company_State__c", "DC");
        private static TextField _customerZipcode = new("Company Postal Code", "Company_Postal_Code__c", "20546");

        public static Address CustomerAddress = new(
            _customerAccountName.Value!,
            _customerAddressLine1.Value!,
            _customerAddressLine2.Value,
            _customerCity.Value!,
            _customerState.Value!,
            _customerZipcode.Value!);
    }
}
