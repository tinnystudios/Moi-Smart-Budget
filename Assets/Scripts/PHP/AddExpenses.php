<?php

$servername = "localhost";
$username = "root";
$password = "Joulszim1";
$dbname = "homedb";


$testVariable = $_POST["jsonObject"];
//print "You entered $testVariable";

$obj = json_decode($testVariable);
$Name = $obj->Name;
$BudgetId = $obj->BudgetId;
$Cost = $obj->Cost;

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);
// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$sql = "INSERT INTO expenses (name, budgetId, cost)
VALUES ('".$Name."', '".$BudgetId."', '".$Cost."')";

if ($conn->query($sql) === TRUE) {
    echo "New record created successfully";
} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}

$conn->close();

?>