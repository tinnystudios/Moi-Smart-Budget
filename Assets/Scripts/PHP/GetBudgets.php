<?php

$servername = "localhost";
$username = "root";
$password = "Joulszim1";
$dbname = "homedb";

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);
// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT * FROM budgets";
$result = $conn->query($sql);

$list = array();
if ($result->num_rows > 0) {
    // output data of each row
    while($row = $result->fetch_assoc()) {
		$list[] = array('id' => $row["id"], 'name' => $row["name"], 'amount' => $row["amount"], 'startTime' => $row["startTime"], 'endTime' => $row["endTime"], 'repeat' => $row["repeat"]);
    }
} else {
    echo "0 results";
}

echo json_encode($list);

$conn->close();
?>