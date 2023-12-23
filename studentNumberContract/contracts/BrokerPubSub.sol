// SPDX-License-Identifier: MIT
pragma solidity >= 0.5.0;
pragma experimental ABIEncoderV2;

contract MessageBroker {
    event TopicCreated(string topicName);
    event PublisherAdvertised(address indexed publisher, string topicName);
    event SubscriberSubscribed(address indexed subscriber, string topicName);
    event MessagePublished(address indexed publisher, string topicName, string message);
    event MessageReceived(string topicName, string message, address indexed subscriber);
    event SubscriberUnsubscribed(address indexed subscriber, string topicName, uint remainingBalance);

    struct Topic {
        string name;
        string[] messages;
        address[] publishers;
        address[] subscribers;
        mapping(address => string[]) subscriberToMessages;
        mapping(address => uint) subscriberToBalance;
    }

    mapping(string => Topic) private topics;
    mapping(address => string) private subscriberToIpMapping;

    function advertise(string memory topicName) public {
        Topic storage topic = topics[topicName];

        if (bytes(topic.name).length == 0) {
            topic.name = topicName;
            emit TopicCreated(topicName);
        }

        topic.publishers.push(msg.sender);
        emit PublisherAdvertised(msg.sender, topicName);
    }

    function subscribe(string memory topicName, string memory ipAddress) public payable {
        Topic storage topic = topics[topicName];
        require(msg.value == 0.5 ether, "Deposit must be 0.5 ether");

        topic.subscribers.push(msg.sender);
        topic.subscriberToBalance[msg.sender] += msg.value;
        subscriberToIpMapping[msg.sender] = ipAddress;

        emit SubscriberSubscribed(msg.sender, topicName);
    }

    function publish(string memory topicName, string memory message) public {
        Topic storage topic = topics[topicName];
        require(isPublisherInTopic(msg.sender, topic), "Publisher not in topic");

        topic.messages.push(message);
        for (uint i = 0; i < topic.subscribers.length; i++) {
            address subscriber = topic.subscribers[i];
            if (topic.subscriberToBalance[subscriber] >= 0.005 ether) {
                topic.subscriberToBalance[subscriber] -= 0.005 ether;
                topic.subscriberToMessages[subscriber].push(message);
                emit MessageReceived(topicName, message, subscriber);
            }
        }

        emit MessagePublished(msg.sender, topicName, message);
    }

    function unsubscribe(string memory topicName) public {
        Topic storage topic = topics[topicName];
        require(isSubscriberInTopic(msg.sender, topic), "Subscriber not in topic");

        uint remainingBalance = topic.subscriberToBalance[msg.sender];
        deleteFromArray(topic.subscribers, msg.sender);

        emit SubscriberUnsubscribed(msg.sender, topicName, remainingBalance);
    }

    function getTopicMessages(string memory topicName) public view returns (string[] memory) {
        return topics[topicName].messages;
    }

    function getSubscriberMessages(string memory topicName) public view returns (string[] memory) {
        return topics[topicName].subscriberToMessages[msg.sender];
    }

    function getSubscriberBalance(string memory topicName) public view returns (uint) {
        return topics[topicName].subscriberToBalance[msg.sender];
    }

    function isPublisherInTopic(address publisher, Topic storage topic) internal view returns (bool) {
        for (uint i = 0; i < topic.publishers.length; i++) {
            if (topic.publishers[i] == publisher) {
                return true;
            }
        }
        return false;
    }

    function isSubscriberInTopic(address subscriber, Topic storage topic) internal view returns (bool) {
        for (uint i = 0; i < topic.subscribers.length; i++) {
            if (topic.subscribers[i] == subscriber) {
                return true;
            }
        }
        return false;
    }

    function deleteFromArray(address[] storage array, address element) internal {
        for (uint i = 0; i < array.length; i++) {
            if (array[i] == element) {
                if (i != array.length - 1) {
                    array[i] = array[array.length - 1];
                }
                array.pop();
                break;
            }
        }
    }
}
