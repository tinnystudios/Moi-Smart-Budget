<?php

$servername = "localhost";
$username = "root";
$password = "Joulszim1";
$dbname = "homedb";

$testVariable = $_POST["jsonObject"];
$obj = json_decode($testVariable);

$Id = $obj->Id;

// Updatable values
$Name = $obj->Name;
$Amount = $obj->Amount;
$StartDate = $obj->StartDate;
$EndDate = $obj->EndDate;
$Repeat = $obj->Repeat;

settype($Id, "integer"); 

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);
// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$sql = "UPDATE budgets SET Amount='".$Amount."', Name = '"$Name"', StartDate = '"$StartDate"', EndDate = '"$EndDate"', Repeat = '"$Repeat"' WHERE id='".$Id."'";

if ($conn->query($sql) === TRUE) {
    echo "Successfully updated budget";
} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}

$conn->close();
?>