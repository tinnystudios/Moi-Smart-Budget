<?php

$servername = "localhost";
$username = "root";
$password = "Joulszim1";
$dbname = "homedb";

$testVariable = $_POST["jsonObject"];

$obj = json_decode($testVariable);
$Name = $obj->Name;
$BudgetId = $obj->BudgetId;
$Cost = $obj->Cost;
$PurchaseDate = $obj->PurchaseDate;
$UserId = $obj->UserId;

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);
// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$sql = "INSERT INTO expensesHistory (name, budgetId, cost, purchaseDate, userId)
VALUES ('".$Name."', '".$BudgetId."', '".$Cost."', '".$PurchaseDate."', '".$UserId."')";

if ($conn->query($sql) === TRUE) {
    echo "New record created successfully";
} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}

$conn->close();

?>