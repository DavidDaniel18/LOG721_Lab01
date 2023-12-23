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

console.log('init contract')
const messageBrokerContract = new web3.eth.Contract(brokerContractABI, contractAddress);

async function main() {
  // Subscribe to the MessagePublished event
  console.log('message published')
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
  console.log('subscriber subscribed')
  messageBrokerContract.events.SubscriberSubscribed({
    fromBlock: 'latest'
  }, (error, event) => {
    if (error) {
      console.error('Error:', error);
    } else {
      console.log('Subscriber Subscribed:', event.returnValues);
    }
  });

  console.log('publisher advertised')
  messageBrokerContract.events.PublisherAdvertised({
    fromBlock: 'latest'
  }, (error, event) => {
    if (error) {
      console.error('Error:', error);
    } else {
      console.log('Publisher Advertised:', event.returnValues);
    }
  });

  const topic = "topic1";

  try {
    const gasEstimate = await studentContract.methods
      .advertise(topic)
      .estimateGas({
        from: accountAddress,
        value: 5400000000000000,
      });

    const result = await studentContract.methods
      .advertise(topic)
      .send({
        from: accountAddress,
        gas: gasEstimate,
        value: 5400000000000000,
      });

    console.log('Transaction hash:', result.transactionHash);
  } catch (error) {
    console.error('Error setting student number:', error);
  }
}

main();