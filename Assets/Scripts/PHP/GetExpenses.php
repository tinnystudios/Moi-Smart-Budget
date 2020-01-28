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

$sql = "SELECT * FROM expenses";
$result = $conn->query($sql);

$list = array();
if ($result->num_rows > 0) {
    // output data of each row
    while($row = $result->fetch_assoc()) {
		$list[] = array('id' => $row["id"], 'name' => $row["name"], 'budgetId' => $row["budgetId"], 'cost' => $row["cost"], 'userId' => $row["userId"], 'purchaseDate' => $row["purchaseDate"]);
    }
} else {
    echo "0 results";
}

echo json_encode($list);

$conn->close();
?>