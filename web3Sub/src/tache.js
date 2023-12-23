const { Web3 } = require('web3')
const fs = require('fs')
require('dotenv').config()

const port = process.env.PORT || 7545
const contractJsonFilePath = process.env.CONTRACT_JSON_PATH
const contractAddress = process.env.CONTRACT_ADDRESS
const accountAddress = process.env.ACCOUNT_ADDRESS

const url = 'http://127.0.0.1:' + port;

const web3 = new Web3(url)
web3.defaultAccount = accountAddress;
web3.defaultNetworkId = 5777;

const contractJSON = JSON.parse(fs.readFileSync(contractJsonFilePath, 'utf-8'))
const brokerContractABI = new web3.eth.Contract(contractJSON.abi, contractAddress)

console.log(`Address: ${url}`);
console.log(`Contract Json Path: ${process.env.CONTRACT_JSON_PATH}`);
console.log(`Contract Address: ${process.env.CONTRACT_ADDRESS}`);
console.log(`Account Address: ${process.env.ACCOUNT_ADDRESS}`);
console.log('')

const messageBrokerContract = new web3.eth.Contract(brokerContractABI, contractAddress);

// Subscribe to the MessagePublished event
messageBrokerContract.events.MessagePublished({
  fromBlock: 'latest'
}, (error, event) => {
  if (error) {
    console.error('Error:', error);
  } else {
    console.log('Message Published:', event.returnValues);
  }
});

// Subscribe to the SubscriberSubscribed event
messageBrokerContract.events.SubscriberSubscribed({
  fromBlock: 'latest'
}, (error, event) => {
  if (error) {
    console.error('Error:', error);
  } else {
    console.log('Subscriber Subscribed:', event.returnValues);
  }
});

// ... (Subscribe to other events as needed)
