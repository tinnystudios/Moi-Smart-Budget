<?php

$servername = "localhost";
$username = "root";
$password = "Joulszim1";
$dbname = "homedb";


$testVariable = $_POST["jsonObject"];
//print "You entered $testVariable";

$obj = json_decode($testVariable);
$Name = $obj->Name;
$Amount = $obj->Amount;

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);
// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$sql = "INSERT INTO budgets (name, amount)
VALUES ('".$Name."', '".$Amount."')";

if ($conn->query($sql) === TRUE) {
    echo "New record created successfully";
} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}

$conn->close();
?>