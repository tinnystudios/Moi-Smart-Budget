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
$Id = $obj->Id;

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);
// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$sql = "DELETE from expenses WHERE id='".$Id."'";

if ($conn->query($sql) === TRUE) {
    echo "Successfully deleted ".$Id." ";
} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}

$conn->close();

?>