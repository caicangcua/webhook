<?php
header('Content-Type: text/html; charset=utf-8');

if(isset($_GET['hub_verify_token'])){ 
    if($_GET['hub_verify_token'] === '123456789'){
    	echo $_GET['hub_challenge'];
        return true;
    } 
    else{
    	echo 'Invalid Verify Token';
        return false;
    }
}

$input = json_decode(file_get_contents('php://input'), true);

if(isset($input['entry'][0]['messaging'][0]['sender']['id'])){

    $sender = $input['entry'][0]['messaging'][0]['sender']['id'];
    $message = $input['entry'][0]['messaging'][0]['message']['text']; 

    $url = 'https://graph.facebook.com/v2.6/me/messages?access_token=EAAX3lZAeXEgMBAOOkAOxF0ZB1WVpvxrSfQKdOrUTPrwMnmv2bNZC45ZBrKTLdbI8UnC3pqn9UjgVC2cT4FJSlwISy1B50SoVCstNqFsY8banpzpCua8pZBdexgg2lsROhTaRUbCdfIhZAkQdErYhdKhY6aa50d9LJwhcjWTj0gIlX8Srraza4y';

    $ch = curl_init($url);
   
    $jsonData = '{
    "recipient":{
        "id":"' . $sender . '"
        },';

    $message_chuan_hoa = mb_strtolower($message);

    if(strpos($message_chuan_hoa, 'xin chào') !== false || strpos($message_chuan_hoa, 'chào') !== false ){
	    $jsonData.= '"message":{
	            "text":"Chào anh!"
	        }
	    }';
	}
	
    curl_setopt($ch, CURLOPT_POST, 1);
    curl_setopt($ch, CURLOPT_POSTFIELDS, $jsonData);
    curl_setopt($ch, CURLOPT_HTTPHEADER, array('Content-Type: application/json'));

    if(!empty($message)){
        $result = curl_exec($ch); 
    }
}
?>