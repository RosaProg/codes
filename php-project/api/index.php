<?php
	$servername = "localhost";
	$username = "root";
	$password = "";
	$dbname = "ziksa";
	
	// Create connection
	$conn = new mysqli($servername, $username, $password, $dbname);

	// Check connection
	if ($conn->connect_error) {
		//echo ("Connection failed: " . $conn->connect_error);
	}
	//echo "Connected successfully";
	
	if($_POST['q'])
	{
		// Se recibieron datos get
		
		switch ($_POST['q']) 
		{
			case 'login':
				if($_POST['user'] && $_POST['latitude'] && $_POST['longitude'])
				{
					$userid = $_POST['user'];
					$login_timestamp = date('Y-m-d H:i:s');
					$lat = $_POST['latitude'];
					$lon = $_POST['longitude'];
					$sql = "INSERT INTO user_session (user_id, logged_in, latitude, longitude, active) 
							VALUES ('$userid','$login_timestamp', '$lat', '$lon' , 1)";

					if ($conn->query($sql) === TRUE) {
						echo json_encode("Login saved succesfully.");
					} else {
						echo json_encode("Error: " . $sql . " " . $conn->error . "");
					}
				}
				break;
            case 'trainers':
                $sql = "select id, name, username, address, city, state, country, phone, creation_date, rating from ziksa.user where role_id = 1";
                $result = $conn->query($sql);
                $datos = array();
                if ($result->num_rows > 0) {
					while($row = $result->fetch_assoc()) {
                        $r = array(
                           'name' => $row["name"],
                           'username' => $row["username"],
                           'address' => $row["address"],
                           'city' => $row["city"],
                           'state' => utf8_encode($row["state"]),
                           'country' => $row["country"],
                           'phone' => $row["phone"],
                           'creation_date' => $row["creation_date"],
                           'rating' => $row["rating"]
                        );
                        array_push($datos,$r);
					}
					echo json_encode($datos);
				} else {
					echo json_encode(false);
				}
                break;
            case 'save_program':
                $course_name = $_POST['course_name'];
                $course_description = $_POST['course_description'];
                $creation_datetime = date('Y-m-d H:i:s');
                $course_complete = 'false';
                $course_start_date = $_POST['course_start_date'];
                $course_end_date = $_POST['course_end_date'];
                $course_cost = $_POST['course_cost'];
                $course_pass_mark = $_POST['course_pass_mark'];
                $venue = $_POST['venue'];
                $pretest_enabled = $_POST['pretest_enabled'];
                $exams_enabled = $_POST['exams_enabled'];
                $employee_segment = $_POST['employee_segment'];
                $employee_level = $_POST['employee_level'];
                $client = $_POST['course_client'];
                $sql = "insert into ziksa.course (course_name, course_description, creation_datetime, course_complete, course_start_date, course_end_date, course_cost, course_pass_mark, venue, pretest_enabled, exams_enabled, employee_segment, employee_level, client) values ('$course_name', '$course_description', '$creation_datetime', $course_complete, '$course_start_date', '$course_end_date', $course_cost, $course_pass_mark, '$venue', $pretest_enabled, $exams_enabled, '$employee_segment', '$employee_level', '$client')";
                //echo $sql;
                if ($conn->query($sql) === TRUE) {
                    echo true;
                } else {
                    echo json_encode("Error: " . $conn->error);
                }
                
                break;
            
            case 'save_exam':
                $course_id = $_POST['course_id'];
                $exam_title = $_POST['exam_title'];
                $creation_datetime = date('Y-m-d H:i:s');
                $enabled = 'true';
                $exam_start_date = $_POST['exam_start_date'];
                $exam_end_date = $_POST['exam_end_date'];
                $course_unit = $_POST['course_unit'];
                $question = $_POST['question'];
                $answer1 = $_POST['answer1'];
                $answer2 = $_POST['answer2'];
                $answer3 = $_POST['answer3'];
                $answer4 = $_POST['answer4'];
                $answer5 = $_POST['answer5'];
                $correct_answer = $_POST['correct_answer'];
                $sql = "insert into ziksa.exam (exam_title, creation_datetime, enabled, course_id, start_date, end_date, course_unit, question, answer1, answer2, answer3, answer4, answer5, correct_answer) values ('$exam_title', '$creation_datetime', $enabled, $course_id, '$exam_start_date', '$exam_end_date', $course_unit, '$question', '$answer1', '$answer2', '$answer3', '$answer4', '$answer5', '$correct_answer')";
                //echo $sql;
                if ($conn->query($sql) === TRUE) {
                    echo true;
                } else {
                    echo json_encode("Error: " . $conn->error);
                }            
                break;
            
            case 'programs':
                $sql = "select course.id, course.course_name, course_start_date, course_end_date, course_description, venue, course.rating, preview_image, course_complete, user.name from ziksa.course left join user on trainerid = user.id";
                $result = $conn->query($sql);
                if ($result->num_rows > 0) {
					while($row = $result->fetch_assoc()) {
                       $datos[] = $row;
					}
					echo json_encode($datos);
				} else {
					echo json_encode(false);
				}
                break;
            
            case 'attendance':
                $sql = "select user.id as user_id, user.name as user_name, course.id as program_id, course.course_name as program_name, attendance.datetime, action from ziksa.attendance left join ziksa.course on course.id = attendance.course_id left join user on attendance.user_id = user.id";
                $result = $conn->query($sql);
                if ($result->num_rows > 0) {
					while($row = $result->fetch_assoc()) {
                       $datos[] = $row;
					}
					echo json_encode($datos);
				} else {
					echo json_encode(false);
				}
                break;
            
			case 'update_location':
				if($_POST['user'] && $_POST['latitude'] && $_POST['longitude'])
				{
					$userid = $_POST['user'];
					$login_timestamp = date('Y-m-d H:i:s');
					$lat = $_POST['latitude'];
					$lon = $_POST['longitude'];
					$sql = "UPDATE user_session SET latitude='$lat', longitude='$lon' WHERE user_id='$userid' AND active=1";

					if ($conn->query($sql) === TRUE) {
						echo json_encode("User logged out succesfully.");
					} else {
						echo json_encode("Error updating user status: " . $conn->error . "");
					}
				}
				break;
			case 'logout':
				if($_POST['user'])
				{
					$userid = $_POST['user'];
					$logout_timestamp = date('Y-m-d H:i:s');
					$sql = "UPDATE user_session SET logged_out='$logout_timestamp', active=0 WHERE user_id='$userid' AND active=1";
					if ($conn->query($sql) === TRUE) {
						echo json_encode("User logged out succesfully.");
					} else {
						echo json_encode("Error updating user status: " . $conn->error . "");
					}
				}
				break;
			case 'available_areas_today':
				$sql = "SELECT * FROM dynamic_areas where from_date <= '" . date("Y-m-d") . "' AND to_date >= '" . date("Y-m-d") . "'";
				$result = $conn->query($sql);
				if ($result->num_rows > 0) {
					while($row = $result->fetch_assoc()) {
						$datos[] = $row;
					}
					echo json_encode($datos);
				} else {
					echo json_encode("0 results");
				}
				break;
			case 'available_areas_today_by_user':
				if($_POST['user'])
				{
					$cant_areas = 0;
					$userid = $_POST['user'];
					// Check if the user doesn't already have an area assigned, previous to get today's areas
					$sql = "SELECT count(1) as user_areas FROM user_area where user_id='$userid'";
					$result = $conn->query($sql);
					if ($result->num_rows > 0) {
						while($row = $result->fetch_assoc()) {
							$cant_areas = $row['user_areas'];
						}
					}
						$sql = "SELECT * FROM dynamic_areas where from_date <= '" . date("Y-m-d") . "' AND to_date >= '" . date("Y-m-d") . "'";
						$result = $conn->query($sql);
						if ($result->num_rows > 0) {
							$i=0;
							while($row = $result->fetch_assoc()) {
								$i++;
								$datos[] = $row;
							}
							$datos["assigned"] = $cant_areas;
							echo json_encode($datos);
						} else {
							echo json_encode("0 results");
						}
					
				}
				break;
			case 'available_areas_all':
				$sql = "SELECT * FROM dynamic_areas";
				$result = $conn->query($sql);
				if ($result->num_rows > 0) {
					while($row = $result->fetch_assoc()) {
						$datos[] = $row;
					}
					echo json_encode($datos);
				} else {
					echo json_encode("0 results");
				}
				break;
			case 'available_areas_city':
				if($_POST['city'])
				{
					$sql = "SELECT * FROM dynamic_areas where from_date >= '" . date("Y-m-d") . "' AND to_date <= '" . date("Y-m-d") . "' AND city = '" . $_POST['city'] . "'";
					$result = $conn->query($sql);
					if ($result->num_rows > 0) {
						while($row = $result->fetch_assoc()) {
							$datos[] = $row;
						}
						echo json_encode($datos);
					} else {
						echo json_encode("0 results");
					}
				}
				break;
			case 'area_accepted':
				if($_POST['user'] && $_POST['area'])
				{
					$userid = $_POST['user'];
					$areaid = $_POST['area'];
					$quantity_applied = 0;
					$people_required = 0;
					$area_status = 1;
					// Count how many users already applied and if it's less than the quantity specified
					// Check if the area is still open
					$sql = "SELECT count(1) as applied_quantity FROM user_area where area_id = $areaid";
					$result = $conn->query($sql);
					if ($result->num_rows > 0) {
						while($row = $result->fetch_assoc()) {
							$quantity_applied = $row['applied_quantity'];
						}
					}
					$sql = "SELECT no_people_required, status FROM dynamic_areas where id = $areaid";
					$result = $conn->query($sql);
					if ($result->num_rows > 0) {
						while($row = $result->fetch_assoc()) {
							$people_required = $row['no_people_required'];
							$area_status = $row['status'];
						}
					}else {
						echo json_encode("Error: the area does not exist.");
					}
					//Validar que el usuario no tenga ya un area asignada
					if(($quantity_applied < $people_required) && ($area_status==1))
					{
						$cant_areas = 0;
						// Check if the user doesn't already have an area assigned, previous to get today's areas
						$sql = "SELECT count(1) as user_areas FROM user_area where user_id='$userid'";
						$result = $conn->query($sql);
						if ($result->num_rows > 0) {
							while($row = $result->fetch_assoc()) {
								$cant_areas = $row['user_areas'];
							}
						}
						if($cant_areas>0)
						{
							$quantity_applied++;
							$sql = "INSERT INTO user_area (user_id, area_id, status) 
							VALUES ('$userid', $areaid, 1)";

							if ($conn->query($sql) === TRUE) {
								echo json_encode("Applied succesfully to the area. Please remember to be 20 minutes before etc..");
							} else {
								echo json_encode("Error: " . $sql . " " . $conn->error . "");
							}
						}else{
							echo json_encode("The user is already assigned to another area. You cannot apply to more than one area.");
						}
						
						if($quantity_applied>=$people_required)
						{
							//echo "Entro";
							$sql = "UPDATE dynamic_areas SET status=0 WHERE id=$areaid";

							if ($conn->query($sql) === TRUE) {
								echo json_encode("Area closed succesfully.");
							} else {
								echo json_encode("Error updating area status: " . $conn->error . "");
							}
						}
					}
					
					
				}
				break;
			default:
				$endpoints = array(
				"All Areas information"=>"http://vsstechnology.com/deliverytrack/api/?q=available_areas_all",
				"All available areas for today"=>"http://vsstechnology.com/deliverytrack/api/?q=available_areas_today",
				"All areas today by user"=>"http://vsstechnology.com/deliverytrack/api/?q=available_areas_today_by_user&user=[USERNAME]",
				"Available areas by city"=>"http://vsstechnology.com/deliverytrack/api/?q=available_areas_city&city[CITY NAME]"
				);
				echo json_encode($endpoints);
				break;
		}
		
	}else{
				echo '"All Areas information"=>"http://vsstechnology.com/deliverytrack/api/?q=available_areas_all"<br/>';
				echo '"All available areas for today"=>"http://vsstechnology.com/deliverytrack/api/?q=available_areas_today"<br/>';
				echo '"All areas today by user"=>"http://vsstechnology.com/deliverytrack/api/?q=available_areas_today_by_user&user=[USERNAME]"<br/>';
				echo '"Available areas by city"=>"http://vsstechnology.com/deliverytrack/api/?q=available_areas_city&city[CITY NAME]"<br/>';
	}